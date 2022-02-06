using Microsoft.AspNetCore.Mvc;
using SecurityWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurityWebApi.Controllers
{
	[Route("api/security/test-three")]
	[ApiController]
	public class TestThreeController : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TestThreeViewModel>>> GetAllAsync()
		{
			return Ok(new TestThreeViewModel[] { new TestThreeViewModel(), new TestThreeViewModel() });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TestThreeViewModel>> GetAsync(int id)
		{
			return Ok(new TestThreeViewModel());
		}

		[HttpPost]
		public async Task<ActionResult<TestThreeViewModel>> CreateAsync([FromBody] TestThreeViewModel value)
		{
			return Ok(new TestThreeViewModel());
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<TestThreeViewModel>> PutAsync(int id, [FromBody] TestThreeViewModel value)
		{
			return Ok(new TestThreeViewModel());
		}

		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			Ok();
		}
	}
}
