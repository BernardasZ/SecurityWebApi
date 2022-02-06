using Microsoft.AspNetCore.Mvc;
using SecurityWebApi.Models;
using System.Threading.Tasks;

namespace SecurityWebApi.Controllers
{
	[Route("api/security/test-two")]
	[ApiController]
	public class TestTwoController : ControllerBase
	{
		[HttpGet("calculate-something")]
		public async Task<ActionResult<decimal>> CalculateAsync([FromQuery] TestTwoViewModel request)
		{
			return Ok(15m);
		}

		[HttpGet("{randomData}/{somthing}")]
		public async Task<ActionResult<TestTwoViewModel>> GetRandomAsync(string randomData, string somthing)
		{
			return Ok(new TestTwoViewModel());
		}
	}
}
