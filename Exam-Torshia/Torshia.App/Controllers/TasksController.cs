namespace Torshia.App.Controllers
{
    using System;
    using System.Globalization;
    using Extensions;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Implementations;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Controllers;
    using ViewModels.Tasks;

    public class TasksController : Controller
    {
        private readonly ITaskService taskService;

        public TasksController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet]
        public IActionResult Details()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        // TODO add sectors in CreateTaskViewModel
        [HttpPost]
        public IActionResult Create(CreateTaskViewModel model)
        {
            var title = model.Title.UrlDecode();
            var dueDate = DateTime.ParseExact(model.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var description = model.Description.UrlDecode();
            var participants = model.Participants.UrlDecode();

            var task = this.taskService.AddTask(title, dueDate, description, participants);
            this.taskService.AddTaskSectors(model.SectorCustomers, model.SectorMarketing, model.SectorFinances, model.SectorInternal, model.SectorManagement, task.Id);

            return new RedirectResult("/");
        }
    }
}