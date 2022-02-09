using System.Collections.Generic;

namespace Domain.Models
{
	public class Action : BaseModel
	{
		public string ActionName { get; set; }
		public string HttpType { get; set; }
		public string ActionUrl { get; set; }
		public Controller Controller { get; set; }
		public virtual ICollection<Property> Properties { get; set; }
	}
}
