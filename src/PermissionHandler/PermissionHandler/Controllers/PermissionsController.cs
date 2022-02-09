using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using PermissionHandler.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PermissionHandler.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PermissionsController : ControllerBase
	{
		private readonly IPermissionHandlerService _permissionHandlerService;

		public PermissionsController(IPermissionHandlerService permissionHandlerService)
		{
			_permissionHandlerService = permissionHandlerService;
		}

		[HttpGet("scrap/{swaggerUrl}")]
		public async Task<ActionResult<Server>> ScrapApi(string swaggerUrl)
		{
			return Ok(await _permissionHandlerService.ScrappApi(swaggerUrl));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Server>>> GetAll()
		{
			return Ok(await _permissionHandlerService.Read());
		}

		[HttpPost]
		public async Task<ActionResult> Save(Server item)
		{
			await _permissionHandlerService.Save(item);

			return Ok();
		}
	}
}
