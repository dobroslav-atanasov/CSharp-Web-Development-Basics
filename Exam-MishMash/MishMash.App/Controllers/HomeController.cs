namespace MishMash.App.Controllers
{
    using System.Linq;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Method;

    public class HomeController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (this.Identity != null)
            {
                if (this.Identity.Roles.Contains("Admin"))
                {
                    this.Model.Data["Name"] = this.Identity.Username;
                    return this.View("Index-Admin");
                }

                if (this.Identity.Roles.Contains("User"))
                {
                    this.Model.Data["Name"] = this.Identity.Username;
                    return this.View("Index-User");
                }
            }

            return this.View();
        }
    }
}