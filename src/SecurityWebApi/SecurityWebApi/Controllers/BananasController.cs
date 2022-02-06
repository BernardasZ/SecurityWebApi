using Microsoft.AspNetCore.Mvc;
using SecurityWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecurityWebApi.Controllers
{
	[Route("api/security/bananas")]
	[ApiController]
	public class BananasController : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<IEnumerable<BananaViewModel>>> GetAllAsync()
		{
			return Ok(new BananaViewModel[] { new BananaViewModel(), new BananaViewModel() });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BananaViewModel>> GetAsync(int id)
		{
			return Ok(new BananaViewModel());
		}

		[HttpPost]
		public async Task<ActionResult<BananaViewModel>> CreateAsync([FromBody] BananaViewModel value)
		{
			return Ok(new BananaViewModel());
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<BananaViewModel>> PutAsync(int id, [FromBody] BananaViewModel value)
		{
			return Ok(new BananaViewModel());
		}

		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			Ok();
		}
	}
}
