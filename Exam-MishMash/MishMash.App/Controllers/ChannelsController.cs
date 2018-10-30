namespace MishMash.App.Controllers
{
    using System.Linq;
    using System.Text;
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
        private readonly IUserService userService;

        public ChannelsController(IChannelService channelService, IUserService userService)
        {
            this.channelService = channelService;
            this.userService = userService;
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

            var user = this.userService.GetUser(this.Identity.Username);
            var channels = this.channelService.GetYourChannels(user.Id);

            var sb = new StringBuilder();
            var index = 1;
            foreach (var channel in channels)
            {
                sb.AppendLine("<tr class=\"row\">");
                sb.AppendLine($"<th class=\"col-md-1\">{index}</th>");
                sb.AppendLine($"<td class=\"col-md-5\">{channel.Name}</td>");
                sb.AppendLine($"<td class=\"col-md-1\">{channel.Type.ToString()}</td>");
                sb.AppendLine($"<td class=\"col-md-2\">{this.channelService.GetTotalFollowers(channel.Id)}</td>");
                sb.AppendLine($"<td class=\"col-md-2\">");
                sb.AppendLine($"<div class=\"button-holder d-flex justify-content-between\">");
                sb.AppendLine($"<a class=\"btn bg-mishmash text-white\" href=\"/Channels/Unfollow/?id={channel.Id}\" role=\"button\">Unfollow</a>");
                sb.AppendLine($"<a class=\"btn bg-mishmash text-white\" href=\"/Channels/Details/?id={channel.Id}\" role=\"button\">Details</a>");
                sb.AppendLine($"</div>");
                sb.AppendLine("</td>");
                sb.AppendLine("</tr>");

                index++;
            }

            this.Model.Data["FollowChannelsViewModel"] = new FollowChannelsViewModel
            {
                AllChannels = sb.ToString()
            };

            return this.View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            if (this.Identity == null)
            {
                return new RedirectResult("/Users/Login");
            }

            var id = int.Parse(this.Request.QueryData["id"].ToString());

            var channel = this.channelService.GetChannel(id);
            if (channel == null)
            {
                return new RedirectResult("/");
            }

            this.Model.Data["ChannelDetailsViewModel"] = new ChannelDetailsViewModel
            {
                Name = channel.Name,
                Type = channel.Type.ToString(),
                Followers = this.channelService.GetTotalFollowers(id),
                Tags = this.channelService.GetAllTags(id),
                Description = channel.Description
            };

            return this.View();
        }

        [HttpGet]
        public IActionResult Follow()
        {
            if (this.Identity == null)
            {
                return new RedirectResult("/Users/Login");
            }

            var channelId = int.Parse(this.Request.QueryData["id"].ToString());
            var channel = this.channelService.GetChannel(channelId);
            var user = this.userService.GetUser(this.Identity.Username);

            if (channel == null || user == null)
            {
                return new RedirectResult("/");
            }

            this.channelService.FollowChannel(channelId, user.Id);

            return new RedirectResult("/");
        }

        // TODO
        [HttpGet]
        public IActionResult UnFollow()
        {
            return new RedirectResult("/");
        }
    }
}