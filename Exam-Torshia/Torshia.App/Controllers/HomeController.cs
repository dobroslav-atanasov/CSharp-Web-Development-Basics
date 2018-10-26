namespace Torshia.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using Models.Enums;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Controllers;
    using ViewModels.Users;

    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var roles = new HashSet<Role>();
            if (this.Identity != null)
            {
                foreach (var roleString in this.Identity.Roles)
                {
                    if (Enum.TryParse<Role>(roleString, out var role))
                    {
                        roles.Add(role);
                    }
                }
            }


            if (roles.Count != 0)
            {
                this.Model.Data["DisplayUsername"] = new DisplayUsername
                {
                    Username = this.Identity.Username
                };

                if (this.Identity != null && roles.Contains(Role.User))
                {

                    return this.View("Index-User-Logged-In");
                }

                if (this.Identity != null && roles.Contains(Role.Admin))
                {
                    return this.View("Index-Admin-Logged-In");
                }
            }

            return this.View();
        }
    }
}