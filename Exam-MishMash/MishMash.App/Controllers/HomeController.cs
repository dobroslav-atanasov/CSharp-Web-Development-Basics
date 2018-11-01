namespace MishMash.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Base;
    using Models;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Method;
    using ViewModels.Channels;

    public class HomeController : BaseController
    {
        private readonly IChannelService channelService;
        private readonly IUserService userService;

        public HomeController(IChannelService channelService, IUserService userService)
        {
            this.channelService = channelService;
            this.userService = userService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var user = this.userService.GetUser(this.Identity.Username);
            var channelsYour = this.channelService.GetAllFollowChannels();
            var channelsSeeOther = this.channelService.GetAllChannels(1);

            this.Model.Data["ChannelsViewModel"] = new ChannelsViewModel
            {
                YourChannels = this.ExtractAllSeeOtherChannels(channelsYour),
                Suggested = "null",
                SeeOther = this.ExtractAllSeeOtherChannels(channelsSeeOther),
            };

            return this.View();
        }

        private string ExtractAllSeeOtherChannels(List<Channel> channels)
        {
            var sb = new StringBuilder();

            var rows = Math.Ceiling(channels.Count / 5.0);
            var skip = 0;
            var take = 5;

            for (int row = 1; row <= rows; row++)
            {
                sb.AppendLine("<div class=\"tasks-row row\">");
                var selectedChannels = channels.Skip(skip).Take(take).ToList();

                if (selectedChannels.Count == 5)
                {
                    for (int col = 0; col < selectedChannels.Count; col++)
                    {
                        sb.AppendLine($"<div class=\"channel col mx-3 bg-mishmash rounded py-3\">");
                        sb.AppendLine($"<h6 class=\"channel-title text-white text-center my-3\">{selectedChannels[col].Name}</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<h6 class=\"channel-title text-white text-center my-4\">{selectedChannels[col].Type.ToString()} Channel</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<h6 class=\"task-title text-white text-center my-4\">{this.channelService.GetTotalFollowers(selectedChannels[col].Id)} Following</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<div class=\"follow-button-holder d-flex justify-content-between w-50 mx-auto mt-4\">");
                        sb.AppendLine($"<a href=\"/Channels/Follow/?id={selectedChannels[col].Id}\"><h6 class=\"text-center text-white\">Follow</h6></a>");
                        sb.AppendLine($"<a href=\"/Channels/Details/?id={selectedChannels[col].Id}\"><h6 class=\"text-center text-white\">Details</h6></a>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</div>");
                    }
                }
                else
                {
                    for (int col = 0; col < selectedChannels.Count; col++)
                    {
                        sb.AppendLine($"<div class=\"channel col mx-3 bg-mishmash rounded py-3\">");
                        sb.AppendLine($"<h6 class=\"channel-title text-white text-center my-3\">{selectedChannels[col].Name}</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<h6 class=\"channel-title text-white text-center my-4\">{selectedChannels[col].Type.ToString()} Channel</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<h6 class=\"task-title text-white text-center my-4\">{this.channelService.GetTotalFollowers(selectedChannels[col].Id)} Following</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<div class=\"follow-button-holder d-flex justify-content-between w-50 mx-auto mt-4\">");
                        sb.AppendLine($"<a href=\"/Channels/Follow/?id={selectedChannels[col].Id}\"><h6 class=\"text-center text-white\">Follow</h6></a>");
                        sb.AppendLine($"<a href=\"/Channels/Details/?id={selectedChannels[col].Id}\"><h6 class=\"text-center text-white\">Details</h6></a>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</div>");
                    }
                    var remainCols = 5 - selectedChannels.Count;
                    for (int i = 0; i < remainCols; i++)
                    {
                        sb.AppendLine($"<div class=\"col mx-3 rounded py-3\">");
                        sb.AppendLine($"</div>");
                    }
                }


                sb.AppendLine("</div>");
                sb.AppendLine("</br>");

                skip += take;
            }

            return sb.ToString();
        }
    }
}