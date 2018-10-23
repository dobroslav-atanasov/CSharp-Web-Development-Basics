namespace SIS.SecurityDemo.Controllers
{
    using Framework.ActionResults.Contracts;
    using Framework.Attributes.Action;
    using Framework.Controllers;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}