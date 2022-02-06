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

			// Read V3 as YAML
			var openApiDocument = new OpenApiStreamReader().Read(stream, out var diagnostic);
			var components = openApiDocument.Components.Schemas.Values.ToList();

			var server = new OpenApiServer();
			server.Url = openApiDocument.Servers[0].Url;

			var serverUrl = openApiDocument.Servers[0].Url;
			var paths = openApiDocument.Paths
				.Select(x => new
				{
					Operations = x.Value.Operations
						.Select(z => new
						{
							FullPath = x.Key,
							ControllerName = x.Key.Replace($"{serverUrl}/", "").Split("/")[0],
							HttpType = $"Http{z.Key}",
							Response = z.Value.Responses.FirstOrDefault(y => y.Key == "200")
						})
						.ToList()
				})
				.Select(x => new
				{
					Operations = x.Operations.Select(y => new
					{
						y.FullPath,
						y.ControllerName,
						ActionName = y.FullPath.Replace($"{serverUrl}/{y.ControllerName}" == y.FullPath ? y.FullPath : $"{serverUrl}/{y.ControllerName}/", ""),
						y.HttpType,
						Content = y.Response.Value.Content
							.Select(z => new
							{
								//ResponseModelType = z.Value.Schema.Reference?.Id,
								//ResponseModelProperties = z.Value.Schema.Properties.Select(w => new { w.Key, w.Value.Type, w.Value.Format }).ToList(),
								//z.Value.Schema,
								Properties = InitLoadProperties(z.Value.Schema, z.Value.Schema.Reference?.Id)
							})
							.FirstOrDefault()
					})
					.ToList()
				})
				.SelectMany(x => x.Operations)
				.Select(x => new
				{
					x.FullPath,
					x.ControllerName,
					x.ActionName,
					x.HttpType,
					//x.Content?.ResponseModelType,
					//x.Content?.ResponseModelProperties,
					//x.Content?.Schema.Type,
					//x.Content?.Schema.Format,
					x.Content?.Properties
				})
				.ToList();

			Console.WriteLine(JsonSerializer.Serialize(paths, new JsonSerializerOptions { WriteIndented = true, MaxDepth = 10 }));
		}

		private static List<Property> InitLoadProperties(OpenApiSchema schema, string referenceId)
		{
			if (schema.Type == "array")
			{
				return LoadProperties(schema.Items.Properties, schema.Reference?.Id);
			}
			else
			{
				return LoadProperties(schema.Properties, schema.Reference?.Id);
			}
		}

		private static List<Property> LoadProperties(IDictionary<string, OpenApiSchema> schemaProperties, string referenceId)
		{
			var properties = new List<Property>();

			if (schemaProperties != null)
			{
				foreach (var property in schemaProperties)
				{
					var newProperty = new Property();
					newProperty.ReferenceId = property.Value.Reference?.Id;
					newProperty.Name = property.Key;
					newProperty.Type = property.Value.Type;
					newProperty.Format = property.Value.Format;
					newProperty.Properties = property.Value.Items != null ? LoadProperties(property.Value.Items.Properties, property.Value.Reference?.Id) : null;
					properties.Add(newProperty);
				}
			}

			return properties;
		}
	}

	public class OpenApiServer
	{
		public string Url { get; set; }
		public string Name { get; set; }
		public List<Controller> Controllers { get; set; }
	}

	public class Controller
	{
		public string Url { get; set; }
		public string Name { get; set; }
		public List<Action> Actions { get; set; }
	}

	public class Action
	{
		public string Name { get; set; }
		public string ReturType { get; set; }
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
