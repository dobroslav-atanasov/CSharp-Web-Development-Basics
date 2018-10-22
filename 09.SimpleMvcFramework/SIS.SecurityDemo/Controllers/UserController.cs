namespace SIS.SecurityDemo.Controllers
{
    using Framework.ActionResults;
    using Framework.ActionResults.Contracts;
    using Framework.Attributes.Action;
    using Framework.Controllers;
    using Framework.Security;

    public class UserController : Controller
    {
        public IActionResult Login()
        {
            this.SignIn(new IdentityUser
            {
                Username = "Dobri", Password = "1234"
            });

            return this.View();
        }

        [Authorize]
        public IActionResult Authorized()
        {
            this.Model["username"] = this.Identity.Username;

            return new RedirectResult("/Home/Index");
        }
    }
}