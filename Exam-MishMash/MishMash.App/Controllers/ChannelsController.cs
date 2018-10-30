namespace MishMash.App.Controllers
{
    using System.Linq;
    using Base;
    using Extensions;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Implementations;
    using SIS.Framework.Attributes.Method;
    using ViewModels.Channels;

    public class ChannelsController : BaseController
    {
        private readonly IChannelService channelService;

        public ChannelsController(IChannelService channelService)
        {
            this.channelService = channelService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (this.Identity == null)
            {
                return new RedirectResult("/Users/Login");
            }

            if (!this.Identity.Roles.ToList().Contains("Admin"))
            {
                return new RedirectResult("/");
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(ChannelCreateViewModel model)
        {
            var name = model.Name.UrlDecode();
            var description = model.Description.UrlDecode();
            var tagString = model.Tags.UrlDecode();
            var channelType = model.Type;

            this.channelService.AddChannel(name, description, tagString, channelType);

            return new RedirectResult("/");
        }

        [HttpGet]
        public IActionResult Followed()
        {
            if (this.Identity == null)
            {
                return new RedirectResult("/Users/Login");
            }

            return new RedirectResult("/Users/Login");
        }
    }
}