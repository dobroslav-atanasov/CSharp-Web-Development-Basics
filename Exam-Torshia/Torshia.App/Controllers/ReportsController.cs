namespace Torshia.App.Controllers
{
    using System.Linq;
    using System.Text;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Implementations;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Controllers;
    using ViewModels.Reports;

    public class ReportsController : Controller
    {
        private readonly IReportService reportService;
        private readonly ITaskService taskService;

        public ReportsController(IReportService reportService, ITaskService taskService)
        {
            this.reportService = reportService;
            this.taskService = taskService;
        }
        
        [HttpGet]
        public IActionResult All()
        {
            if (!this.Identity.Roles.ToList().Contains("Admin"))
            {
                return new RedirectResult("/");
            }

            var reports = this.reportService.GetAllReports();

            var sb = new StringBuilder();
            var index = 1;
            foreach (var report in reports)
            {
                sb.AppendLine("<tr class=\"row\">");
                sb.AppendLine($"<th class=\"col-md-1\">{index}</th>");
                sb.AppendLine($"<td class=\"col-md-5\">{report.Task.Title}</td>");
                sb.AppendLine($"<td class=\"col-md-1\">{this.taskService.GetTaskLevel(report.TaskId)}</td>");
                sb.AppendLine($"<td class=\"col-md-2\">{report.Status.ToString()}</td>");
                sb.AppendLine($"<td class=\"col-md-2\">");
                sb.AppendLine($"<div class=\"button-holder d-flex justify-content-between\">");
                sb.AppendLine($"<a class=\"btn bg-torshia text-white\" href=\"/Reports/Details/?id={report.TaskId}\" role=\"button\">Details</a>");
                sb.AppendLine($"</div>");
                sb.AppendLine("</td>");
                sb.AppendLine("</tr>");

                index++;
            }

            var str = sb.ToString();

            this.Model.Data["ReportAll"] = new ReportAll
            {
                Report = sb.ToString()
            };

            return this.View();
        }
    }
}