namespace Torshia.App.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Extensions;
    using Models.Enums;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Implementations;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Controllers;
    using ViewModels.Tasks;

    public class TasksController : Controller
    {
        private readonly IUserService userService;
        private readonly ITaskService taskService;
        private readonly IReportService reporterService;

        public TasksController(IUserService userService, ITaskService taskService, IReportService reporterService)
        {
            this.userService = userService;
            this.taskService = taskService;
            this.reporterService = reporterService;
        }
        
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
        public IActionResult Create(CreateTaskViewModel model)
        {
            var title = model.Title.UrlDecode();
            var dueDate = DateTime.ParseExact(model.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var description = model.Description.UrlDecode();
            var participants = model.Participants.UrlDecode();

            var task = this.taskService.AddTask(title, dueDate, description, participants);
            this.taskService.AddTaskSectors(model.SectorCustomers, model.SectorMarketing, model.SectorFinances, model.SectorInternal, model.SectorManagement, task.Id);

            var username = this.Identity.Username;
            var user = this.userService.GetUser(username);

            var date = DateTime.UtcNow.Date;
            var status = this.GenerateStatus();

            this.reporterService.AddReport(date, status, task.Id, user.Id);

            return new RedirectResult("/");
        }

        [HttpGet]
        public IActionResult Details()
        {
            int taskId = int.Parse(this.Request.QueryData["id"].ToString());
            var task = this.taskService.GetTask(taskId);

            this.Model.Data["TaskDetails"] = new TaskDetails()
            {
                Title = task.Title,
                Level = this.taskService.GetTaskLevel(taskId),
                DueDate = task.DueDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Participants = task.Participants,
                AffectedSectors = this.taskService.GetAllAffectedSectors(taskId),
                Description = task.Description
            };

            return this.View();
        }

        [HttpGet]
        public IActionResult Report()
        {
            var taskId = int.Parse(this.Request.QueryData["id"].ToString());
            this.taskService.ChangeIsReported(taskId);
            return  new RedirectResult("/");
        }

        private Status GenerateStatus()
        {
            var randon = new Random();
            var num = randon.Next(1, 5);
            switch (num)
            {
                case 1:
                    return Status.Archived;
                default:
                    return Status.Completed;
            }
        }
    }
}