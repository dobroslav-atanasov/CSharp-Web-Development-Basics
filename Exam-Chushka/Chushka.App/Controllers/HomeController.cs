namespace Chushka.App.Controllers
{
    using System;
    using System.Linq;
    using System.Text;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.Attributes.Method;
    using ViewModels.Products;

    public class HomeController : BaseController
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (this.Identity != null)
            {
                this.Model.Data["AllProductsModel"] = new AllProductsModel
                {
                    Text = this.GenerateHtml()
                };

                if (this.Identity.Roles.Contains("Admin"))
                {
                    return this.View("Index-Admin");
                }

                if (this.Identity.Roles.Contains("User"))
                {
                    this.Model.Data["Name"] = this.Identity.Username;
                    return this.View("Index-User");
                }
            }

            return this.View();
        }

        private string GenerateHtml()
        {
            var sb = new StringBuilder();
            var products = this.productService.GetAllProducts();

            var rows = Math.Ceiling(products.Count / 5.0);
            var skip = 0;
            var take = 5;

            for (int row = 1; row <= rows; row++)
            {
                sb.AppendLine($"<div class=\"row d-flex justify-content-around mt-4\">");
                var selectedProducts = products.Skip(skip).Take(take).ToList();

                for (int col = 0; col < selectedProducts.Count; col++)
                {
                    sb.AppendLine($"<a href=\"/Products/Details/?id={selectedProducts[col].Id}\" class=\"col-md-2\">");
                    sb.AppendLine($"<div class=\"product p-1 chushka-bg-color rounded-top rounded-bottom\">");
                    sb.AppendLine($"<h5 class=\"text-center mt-3\">{selectedProducts[col].Name}</h5>");
                    sb.AppendLine($"<hr class=\"hr-1 bg-white\"/>");
                    if (selectedProducts[col].Description.Length > 50)
                    {
                        sb.AppendLine($"<p class=\"text-white text-center\">{selectedProducts[col].Description.Substring(0, 50)}...</p>");
                    }
                    else
                    {
                        sb.AppendLine($"<p class=\"text-white text-center\">{selectedProducts[col].Description}</p>");
                    }
                    sb.AppendLine($"<hr class=\"hr-1 bg-white\"/>");
                    sb.AppendLine($"<h6 class=\"text-center text-white mb-3\">${selectedProducts[col].Price:F2}</h6>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("</a>");
                }

                sb.AppendLine("</div>");
                sb.AppendLine("</br>");
                skip += take;
            }

            return sb.ToString();
        }
    }
}