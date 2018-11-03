namespace MishMash.App.Controllers
{
    using System.Linq;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Implementations;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Attributes.Method;
    using ViewModels.Channels;

    public class ChannelsController : BaseController
    {
        private readonly IChannelService channelService;
        private readonly IUserService userService;

        public ChannelsController(IChannelService channelService, IUserService userService)
        {
            this.channelService = channelService;
            this.userService = userService;
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Create(ChannelCreateViewModel model)
        {
            this.channelService.AddChannel(model.Name, model.Description, model.Tags, model.Type);
            return new RedirectResult("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Followed()
        {
            var user = this.userService.GetUserByUsername(this.Identity.Username);
            if (user == null)
            {
                return new RedirectResult("/");
            }

            var channels = this.channelService
                .GetYourChannels(user.Id)
                .Select(c => new AllFollowedChannels
                {
                    Id = c.Id,
                    Name = c.Name,
                    Followers = this.channelService.GetTotalFollowers(c.Id),
                    Type = c.Type.ToString()
                })
                .ToList();

            for (int i = 0; i < channels.Count; i++)
            {
                channels[i].Index = i + 1;
            }

            this.Model.Data["Channels"] = channels;
            return this.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details()
        {
            var channelId = int.Parse(this.Request.QueryData["id"].ToString());
            var channel = this.channelService.GetChannelById(channelId);
            if (channel == null)
            {
                return new RedirectResult("/");
            }

            this.Model.Data["ChannelDetails"] = new ChannelDetails
            {
                Name = channel.Name,
                Type = channel.Type.ToString(),
                Followers = this.channelService.GetTotalFollowers(channelId),
                Tags = this.channelService.GetAllTags(channelId),
                Description = channel.Description
            };

            return this.View();
        }
    }
}