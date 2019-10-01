using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Workforce.Models
{
	public class UserAuthentication : ActionFilterAttribute, IAuthorizationFilter
	{
		private string v;

		public UserAuthentication(string v)
		{
			this.v = v;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (!HttpContext.Current.User.Identity.IsAuthenticated)
			{
				filterContext.Result = new HttpUnauthorizedResult();
			}
			if (HttpContext.Current.User.Identity is FormsIdentity)
			{
				FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
				FormsAuthenticationTicket ticket = id.Ticket;
				string userData = ticket.UserData;
				string[] roles = userData.Split(',');

				string[] allowRoles = this.v.Split(',');
				if(roles.Intersect(allowRoles).Any())
				{
					HttpContext.Current.User = new GenericPrincipal(id, roles);
				}
				else
				{
					filterContext.Result = new RedirectResult("/Home/AccessDenied");
				}
			}
		}
	}
}		