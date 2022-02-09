using System.Collections.Generic;

namespace Domain.Models
{
	public class Controller : BaseModel
	{
		public string ControllerUrl { get; set; }
		public string ControllerName { get; set; }
		public long ServerId { get; set; }
		public Server Server { get; set; }
		public virtual ICollection<Action> Actions { get; set; }
	}
}
