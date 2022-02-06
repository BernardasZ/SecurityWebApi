using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace SecurityWebApi.FilterAttributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class SecurityResourceFilterAttribute : Attribute, IResourceFilter
	{
		public void OnResourceExecuting(ResourceExecutingContext context)
		{
		}

		public void OnResourceExecuted(ResourceExecutedContext context)
		{
			var value = (context.Result as OkObjectResult)?.Value;

			if (value != null)
			{
				var controller = context.RouteData.Values["controller"].ToString();
				var action = context.RouteData.Values["action"].ToString();
			}
		}
	}
}
