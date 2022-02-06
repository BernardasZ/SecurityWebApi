using Microsoft.AspNetCore.Mvc;
using SecurityWebApi.Models;
using System.Threading.Tasks;

namespace SecurityWebApi.Controllers
{
	[Route("api/security/calculate")]
	[ApiController]
	public class CalculateController : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<decimal>> CalculateAsync([FromQuery] RandomCalculationViewModel request)
		{
			return Ok(15m);
		}

		[HttpGet("{randomData}/{multiplier}")]
		public async Task<ActionResult<RandomCalculationViewModel>> GetRandomAsync(string randomData, string somthing)
		{
			return Ok(new RandomCalculationViewModel());
		}
	}
}
