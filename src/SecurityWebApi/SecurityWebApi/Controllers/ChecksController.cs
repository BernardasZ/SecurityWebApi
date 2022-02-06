using Microsoft.AspNetCore.Mvc;
using SecurityWebApi.FilterAttributes;
using SecurityWebApi.Models;
using System.Collections.Generic;

namespace SecurityWebApi.Controllers
{
	[Route("api/security/checks")]
	[ApiController]
	public class ChecksController : ControllerBase
	{
		[HttpGet("{id}")]
		[SecurityResourceFilter]
		public ActionResult<UserDataViewModel> GetData(int id)
		{
			return Ok(new UserDataViewModel
			{
				Id = 1,
				Name = "Bernardas",
				Surname = "Zokas",
				Address = new AddressViewModel
				{
					Country = "Lithuania",
					City = "Vilnius",
					Street = "Sausio 13'osios",
					HouseNumber = "11-50"
				},
				Phones = new List<PhoneViewModel>
				{
					new PhoneViewModel
					{
						Number = "868888888",
						Type =  PhoneNumberType.Mobile
					}
				}
			});
		}

		[HttpGet]
		public ActionResult<List<long>> GetData()
		{
			return Ok(new List<long>());
		}

		[HttpPost]
		public ActionResult<EmptyViewModel> CreateEmpty()
		{
			return Ok(new EmptyViewModel());
		}
	}
}
