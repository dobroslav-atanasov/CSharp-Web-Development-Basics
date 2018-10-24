namespace IRunes.App.Controllers
{
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Controllers;
    using ViewModels;

    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (this.Identity() != null)
            {
                this.Model.Data["UserViewModel"] = new UserViewModel
                {
                    Username = this.Identity().Username,
                };

                return this.View("Index-Logged-In");
            }

            return this.View();
        }
    }
}