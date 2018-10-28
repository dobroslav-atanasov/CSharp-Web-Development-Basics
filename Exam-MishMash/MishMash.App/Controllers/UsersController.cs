namespace MishMash.App.Controllers
{
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Controllers;

    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }
    }
}