using Microsoft.AspNetCore.Mvc;
using SecurityWebApi.FilterAttributes;
using SecurityWebApi.Models;

namespace SecurityWebApi.Controllers
{
	[Route("api/security/checks")]
	[ApiController]
	public class ChecksController : ControllerBase
	{
		[HttpGet("{id}")]
		[SecurityResourceFilter]
		public ActionResult<TestOneViewModel> GetData(int id)
		{
			return Ok(new TestOneViewModel
			{
				Id = 1,
				Name = "Bernardas",
				Surname = "Zokas",
				Country = "Lithuania",
				City = "Vilnius",
				Street = "Sausio 13'osios",
				HouseNumber = "11-50"
			});
		}
	}
}
