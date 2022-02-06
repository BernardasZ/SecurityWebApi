using Microsoft.AspNetCore.Mvc;
using SecurityWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurityWebApi.Controllers
{
	[Route("api/security/apples")]
	[ApiController]
	public class ApplesController : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<IEnumerable<AppleViewModel>>> GetAllAsync()
		{
			return Ok(new AppleViewModel[] { new AppleViewModel(), new AppleViewModel() });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<AppleViewModel>> GetAsync(int id)
		{
			return Ok(new AppleViewModel());
		}

		[HttpPost]
		public async Task<ActionResult<AppleViewModel>> CreateAsync([FromBody] AppleViewModel value)
		{
			return Ok(new AppleViewModel());
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<AppleViewModel>> PutAsync(int id, [FromBody] AppleViewModel value)
		{
			return Ok(new AppleViewModel());
		}

		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			Ok();
		}
	}
}
