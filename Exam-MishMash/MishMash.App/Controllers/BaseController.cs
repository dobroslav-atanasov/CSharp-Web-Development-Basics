namespace MishMash.App.Controllers
{
    using System.Linq;
    using System.Runtime.CompilerServices;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Controllers;

    public abstract class BaseController : Controller
    {
        protected override IViewable View([CallerMemberName] string actionName = "")
        {
            this.Model.Data["Guest"] = "none";
            this.Model.Data["Admin"] = "none";
            this.Model.Data["User"] = "none";

            if (this.Identity == null)
            {
                this.Model.Data["Guest"] = "block";
            }
            else if (this.Identity != null && this.Identity.Roles.Contains("Admin"))
            {
                this.Model.Data["Admin"] = "block";
            }
            else if (this.Identity != null && this.Identity.Roles.Contains("User"))
            {
                this.Model.Data["User"] = "block";
            }

            return base.View(actionName);
        }
    }
}