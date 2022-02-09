using System.Collections.Generic;

namespace Domain.Models
{
	public class Server : BaseModel
	{
		public string ApiUrl { get; set; }
		public string ApiName { get; set; }
		public virtual ICollection<Controller> Controllers { get; set; }
	}
}
