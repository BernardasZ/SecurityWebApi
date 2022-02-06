using System.Collections.Generic;

namespace SecurityWebApi.Models
{
	public class UserDataViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public AddressViewModel Address { get; set; }
		public List<PhoneViewModel> Phones { get; set; }
		public List<string> Emails { get; set; }
	}
}