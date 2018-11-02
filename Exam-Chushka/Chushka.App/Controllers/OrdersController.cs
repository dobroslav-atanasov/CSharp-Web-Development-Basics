namespace Chushka.App.Controllers
{
    using System.Globalization;
    using System.Text;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Attributes.Method;
    using ViewModels.Orders;

    public class OrdersController : BaseController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        
        [HttpGet]
        [Authorize("Admin")]
        public IActionResult All()
        {
            var orders = this.orderService.GetAllOrders();
            var sb = new StringBuilder();
            var index = 1;
            foreach (var order in orders)
            {
                sb.AppendLine("<tr class=\"row\">");
                sb.AppendLine($"<th class=\"col-md-1\">{index}</th>");
                sb.AppendLine($"<td class=\"col-md-2\">{order.Id}</td>");
                sb.AppendLine($"<td class=\"col-md-3\">{order.Client.Username}</td>");
                sb.AppendLine($"<td class=\"col-md-3\">{order.Product.Name}</td>");
                sb.AppendLine($"<td class=\"col-md-3\">{order.OrderedOn.ToString("hh:mm dd/MM/yyyy", CultureInfo.InvariantCulture)}</td>");
                sb.AppendLine("</tr>");

                index++;
            }

            this.Model.Data["AllOrdersModel"] = new AllOrdersModel
            {
                Text = sb.ToString()
            };
            return this.View();
        }
    }
}