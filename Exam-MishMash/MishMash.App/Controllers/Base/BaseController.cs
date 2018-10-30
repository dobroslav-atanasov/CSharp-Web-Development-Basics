namespace MishMash.App.Controllers.Base
{
    using System.Linq;
    using System.Runtime.CompilerServices;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Controllers;

    public class BaseController : Controller
    {
        protected override IViewable View([CallerMemberName] string actionName = "")
        {
            if (this.Identity == null)
            {
                this.Model.Data["Guest"] = "block";
                this.Model.Data["User"] = "none";
                this.Model.Data["Admin"] = "none";
            }
            else if (this.Identity != null && this.Identity.Roles.Contains("Admin"))
            {
                this.Model.Data["Admin"] = "block";
                this.Model.Data["Name"] = this.Identity.Username;
                this.Model.Data["Guest"] = "none";
                this.Model.Data["User"] = "none";
            }
            else
            {
                this.Model.Data["User"] = "block";
                this.Model.Data["Name"] = this.Identity.Username;
                this.Model.Data["Guest"] = "none";
                this.Model.Data["Admin"] = "none";

            }

            return base.View(actionName);
        }
    }
}