using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenApiScraper
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri("https://localhost:44305/api/security/")
			};

			var stream = await httpClient.GetStreamAsync("swagger/v1/swagger.json");
			var openApiDocument = new OpenApiStreamReader().Read(stream, out var diagnostic);
			var components = openApiDocument.Components.Schemas.Values.ToList();
			var serverUrl = openApiDocument.Servers[0].Url;

			var scrappedControllers = openApiDocument.Paths
				.Select(x => new
				{
					Operations = x.Value.Operations.Select(z => new
					{
						FullPath = x.Key,
						ControllerName = x.Key.Replace($"{serverUrl}/", "").Split("/")[0],
						HttpType = $"Http{z.Key}",
						Response = z.Value.Responses.FirstOrDefault(y => y.Key == "200"),
						ControllerTag = z.Value.Tags.FirstOrDefault()?.Name
					})
					.ToList()
				})
				.Select(x => new
				{
					Operations = x.Operations.Select(y => new
					{
						y.FullPath,
						y.ControllerTag,
						y.ControllerName,
						ActionName = y.FullPath.Replace($"{serverUrl}/{y.ControllerName}" == y.FullPath
							? y.FullPath
							: $"{serverUrl}/{y.ControllerName}/", ""),
						y.HttpType,
						Content = y.Response.Value.Content
						.Select(z => new
						{
							Properties = InitLoadProperties(z.Value.Schema, z.Value.Schema.Reference?.Id)
						})
						.FirstOrDefault()
					})
					.ToList()
				})
				.SelectMany(x => x.Operations)
				.GroupBy(x => $"{serverUrl}/{x.ControllerName}")
				.Select(x => new Controller
				{
					ControllerName = x.FirstOrDefault()?.ControllerTag,
					ControllerUrl = x.Key,
					Actions = x.Select(z => new Action
					{
						ActionName = z.ActionName,
						ActionUrl = z.FullPath,
						HttpType = z.HttpType,
						Properties = z.Content?.Properties
					})
						.ToList()
				})
				.ToList();

			var scrappedApi = new OpenApiServer
			{
				ApiUrl = openApiDocument.Servers[0].Url,
				ApiName = openApiDocument.Info.Title,
				Controllers = scrappedControllers
			};

			Console.WriteLine(JsonSerializer.Serialize(scrappedApi, new JsonSerializerOptions { WriteIndented = true, MaxDepth = 10 }));
		}

		private static List<Property> InitLoadProperties(OpenApiSchema schema, string referenceId)
		{
			if (schema.Type == "array")
			{
				return schema.Items.Properties.Count > 0
					? LoadProperties(schema.Items.Properties, schema.Items.Reference?.Id)
					: new List<Property> { CreateNewProperty(schema.Items, null, schema.Items.Reference?.Id) };
			}
			else if (schema.Type == "object" && schema.Properties.Count > 0)
			{
				return LoadProperties(schema.Properties, referenceId);
			}

			return new List<Property> { CreateNewProperty(schema, null, referenceId) };
		}

		private static List<Property> LoadProperties(IDictionary<string, OpenApiSchema> schema, string referenceId)
		{
			var properties = new List<Property>();

			foreach (var property in schema)
			{
				properties.Add(CreateNewProperty(property.Value, property.Key, referenceId));
			}

			return properties;
		}

		private static Property CreateNewProperty(OpenApiSchema schema, string name, string referenceId)
		{
			var property = new Property();
			property.ReferenceId = referenceId;
			property.Name = name;
			property.Format = schema.Format;

			property.Type = schema.Type == "array"
				? schema.Items.Reference?.Id
				: schema.Type == "object"
					? schema.Reference?.Id
					: schema.Type;

			property.Properties = schema.Properties.Count > 0
				? InitLoadProperties(schema, schema.Reference?.Id)
				: schema.Items != null
					? InitLoadProperties(schema.Items, schema.Items.Reference?.Id)
					: null;

			return property;
		}
	}

	public class OpenApiServer
	{
		public string ApiUrl { get; set; }
		public string ApiName { get; set; }
		public List<Controller> Controllers { get; set; }
	}

	public class Controller
	{
		public string ControllerUrl { get; set; }
		public string ControllerName { get; set; }
		public List<Action> Actions { get; set; }
	}

	public class Action
	{
		public string ActionName { get; set; }
		public string HttpType { get; set; }
		public string ActionUrl { get; set; }
		public List<Property> Properties { get; set; }
	}

	public class Property
	{
		public string ReferenceId { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Format { get; set; }
		public List<Property> Properties { get; set; }
	}
}
