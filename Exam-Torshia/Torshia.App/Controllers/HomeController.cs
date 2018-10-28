namespace Torshia.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Models.Enums;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Controllers;
    using ViewModels.Tasks;
    using ViewModels.Users;

    public class HomeController : Controller
    {
        private readonly ITaskService taskService;

        public HomeController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var roles = new HashSet<Role>();
            if (this.Identity != null)
            {
                foreach (var roleString in this.Identity.Roles)
                {
                    if (Enum.TryParse<Role>(roleString, out var role))
                    {
                        roles.Add(role);
                    }
                }
            }

            if (roles.Count != 0)
            {
                this.Model.Data["DisplayUsername"] = new DisplayUsername
                {
                    Username = this.Identity.Username
                };

                this.Model.Data["DisplayTasks"] = new DisplayTasks
                {
                    Tasks = this.LoadAllTasks()
                };

                if (this.Identity != null && roles.Contains(Role.User))
                {
                    return this.View("Index-User-Logged-In");
                }

                if (this.Identity != null && roles.Contains(Role.Admin))
                {
                    return this.View("Index-Admin-Logged-In");
                }
            }

            return this.View();
        }

        private string LoadAllTasks()
        {
            var tasks = this.taskService.GetAllNonReportedTasks();
            var sb = new StringBuilder();

            var rows = Math.Ceiling(tasks.Count / 5.0);
            var skip = 0;
            var take = 5;

            for (int row = 1; row <= rows; row++)
            {
                sb.AppendLine("<div class=\"tasks-row row\">");
                var selectedTasks = tasks.Skip(skip).Take(take).ToList();

                if (selectedTasks.Count == 5)
                {
                    for (int col = 0; col < selectedTasks.Count; col++)
                    {
                        sb.AppendLine($"<div class=\"task col mx-3 bg-torshia rounded py-3\">");
                        sb.AppendLine($"<h6 class=\"task-title text-white text-center my-3\">{selectedTasks[col].Title}</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<h6 class=\"task-title text-white text-center my-4\">Level: {this.taskService.GetTaskLevel(selectedTasks[col].Id)}</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<div class=\"follow-button-holder d-flex justify-content-between w-50 mx-auto mt-4\">");
                        sb.AppendLine($"<a href=\"/Tasks/Report/?id={selectedTasks[col].Id}\"><h6 class=\"text-center text-white\">Report</h6></a>");
                        sb.AppendLine($"<a href=\"/Tasks/Details/?id={selectedTasks[col].Id}\"><h6 class=\"text-center text-white\">Details</h6></a>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</div>");
                    }
                }
                else
                {
                    for (int col = 0; col < selectedTasks.Count; col++)
                    {
                        sb.AppendLine($"<div class=\"task col mx-3 bg-torshia rounded py-3\">");
                        sb.AppendLine($"<h6 class=\"task-title text-white text-center my-3\">{selectedTasks[col].Title}</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<h6 class=\"task-title text-white text-center my-4\">Level: {this.taskService.GetTaskLevel(selectedTasks[col].Id)}</h6>");
                        sb.AppendLine($"<hr class=\"bg-white hr-2 w-75\">");
                        sb.AppendLine($"<div class=\"follow-button-holder d-flex justify-content-between w-50 mx-auto mt-4\">");
                        sb.AppendLine($"<a href=\"/Tasks/Report/?id={selectedTasks[col].Id}\" ><h6 class=\"text-center text-white\">Report</h6></a>");
                        sb.AppendLine($"<a href=\"/Tasks/Details/?id={selectedTasks[col].Id}\"><h6 class=\"text-center text-white\">Details</h6></a>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</div>");
                    }
                    var remainCols = 5 - selectedTasks.Count;
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