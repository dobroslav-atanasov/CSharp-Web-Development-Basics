namespace MishMash.App.Controllers
{
    using System.Linq;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Implementations;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Controllers;
    using ViewModels.Channels;

    public class ChannelsController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            if (!this.Identity.Roles.ToList().Contains("Admin"))
            {
                return new RedirectResult("/");
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(ChannelCreateViewModel model)
        {

            return new RedirectResult("/");
        }
    }
}