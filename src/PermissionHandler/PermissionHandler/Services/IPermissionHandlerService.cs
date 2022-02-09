using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PermissionHandler.Services
{
	public interface IPermissionHandlerService
	{
		Task<Server> ScrappApi(string url);
		Task Save(Server server);
		Task<IEnumerable<Server>> Read();
	}
}
