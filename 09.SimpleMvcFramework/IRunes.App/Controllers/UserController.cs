namespace IRunes.App.Controllers
{
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Controllers;
    using ViewModels;

    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterViewModel model)
        {
            return null;
        }
    }
}