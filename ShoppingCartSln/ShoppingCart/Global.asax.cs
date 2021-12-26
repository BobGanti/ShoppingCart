using ShoppingCart.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ShoppingCart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // **** Enabling user roles - Bobga ********
        protected void Application_AuthenticateRequest()
        {
            if (User == null) return;
            string username = Context.User.Identity.Name;
            string[] roles = null;
            using (var db = new NtiAppsContext())
            {
                var userInDb = db.Users.SingleOrDefault(u => u.Username == username);
                roles = db.UserRoles.Where(u => u.UserId == userInDb.Id).Select(u => u.Role.Name).ToArray();
            }

            // Build IPrincipal object
            IIdentity userIdentity = new GenericIdentity(username);
            IPrincipal newUserObject = new GenericPrincipal(userIdentity, roles);

            Context.User = newUserObject;
        }
    }
}
