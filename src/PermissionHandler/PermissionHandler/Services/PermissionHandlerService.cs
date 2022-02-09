using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PermissionHandler.Services
{
	public class PermissionHandlerService : IPermissionHandlerService
	{
		private readonly ApplicationDbContext _applicationDbContext;
		public PermissionHandlerService(ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public async Task<Server> ScrappApi(string url)
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
					Actions = x.Select(z => new Domain.Models.Action
					{
						ActionName = z.ActionName,
						ActionUrl = z.FullPath,
						HttpType = z.HttpType,
						Properties = z.Content?.Properties
					})
						.ToList()
				})
				.ToList();

			var scrappedApi = new Server
			{
				ApiUrl = openApiDocument.Servers[0].Url,
				ApiName = openApiDocument.Info.Title,
				Controllers = scrappedControllers
			};

			Console.WriteLine(JsonSerializer.Serialize(scrappedApi, new JsonSerializerOptions { WriteIndented = true, MaxDepth = 10 }));

			return scrappedApi;
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

		public async Task Save(Server server)
		{
			_applicationDbContext.Add(server);
			await _applicationDbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<Server>> Read()
		{
			_applicationDbContext.Properties.Load();

			return await _applicationDbContext.Servers
				.Include(x => x.Controllers)
				.ThenInclude(x => x.Actions)
				.ThenInclude(x => x.Properties)
				.ToListAsync();
		}
	}
}
