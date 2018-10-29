namespace MishMash.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using Models.Enums;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Controllers;
    using ViewModels.Home;

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
                if (this.Identity != null && roles.Contains(Role.User))
                {
                    this.Model.Data["DisplayAdminViewModel"] = new DisplayAdminViewModel { DisplayAdmin = "none" };
                    this.Model.Data["DisplayGuestViewModel"] = new DisplayGuestViewModel { DisplayGuest = "none" };
                    this.Model.Data["DisplayUserViewModel"] = new DisplayUserViewModel { DisplayUser = "block", Username = this.Identity.Username };
                    return this.View();
                }

                if (this.Identity != null && roles.Contains(Role.Admin))
                {
                    this.Model.Data["DisplayUserViewModel"] = new DisplayUserViewModel { DisplayUser = "none" };
                    this.Model.Data["DisplayGuestViewModel"] = new DisplayGuestViewModel { DisplayGuest = "none" };
                    this.Model.Data["DisplayAdminViewModel"] = new DisplayAdminViewModel { DisplayAdmin = "block", Username = this.Identity.Username };
                    return this.View();
                }
            }

            this.Model.Data["DisplayAdminViewModel"] = new DisplayAdminViewModel { DisplayAdmin = "none" };
            this.Model.Data["DisplayUserViewModel"] = new DisplayUserViewModel { DisplayUser = "none" };
            this.Model.Data["DisplayGuestViewModel"] = new DisplayGuestViewModel { DisplayGuest = "block" };

            return this.View();
        }
    }
}