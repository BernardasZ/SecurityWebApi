using System.Collections.Generic;

namespace Domain.Models
{
	public class Property : BaseModel
	{
		public string ReferenceId { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Format { get; set; }
		public Action Action { get; set; }
		public Property Navigation { get; set; }
		public virtual ICollection<Property> Properties { get; set; }
	}
}
