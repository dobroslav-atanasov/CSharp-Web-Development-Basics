namespace Chushka.App.Controllers
{
    using System;
    using System.Linq;
    using Extensions;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Implementations;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Attributes.Method;
    using ViewModels.Products;
    using Type = Models.Enums.Type;

    public class ProductsController : BaseController
    {
        private readonly IProductService productService;
        private readonly IUserService userService;

        public ProductsController(IProductService productService, IUserService userService)
        {
            this.productService = productService;
            this.userService = userService;
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductModel model)
        {
            var name = model.Name.UrlDecode();
            var price = model.Price;
            var description = model.Description.UrlDecode();
            var type = Enum.Parse<Type>(model.Type);

            this.productService.AddProduct(name, price, description, type);
            return new RedirectResult("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details()
        {
            var productId = int.Parse(this.Request.QueryData["id"].ToString());

            var product = this.productService.GetProductById(productId);
            if (product == null)
            {
                return new RedirectResult("/");
            }

            var admin = string.Empty;
            var user = string.Empty;

            if (this.Identity.Roles.Contains("Admin"))
            {
                admin = "block";
                user = "none";
            }
            else if (this.Identity.Roles.Contains("User"))
            {
                admin = "none";
                user = "block";
            }

            this.Model.Data["ProductDetailsModel"] = new ProductDetailsModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = $"{product.Price:F2}",
                TypeToString = product.Type.ToString(),
                Admin = admin,
                User = user
            };

            return this.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Order()
        {
            var productId = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.productService.GetProductById(productId);
            var user = this.userService.GetUserByUsername(this.Identity.Username);

            if (product == null || user == null)
            {
                return new RedirectResult("/");
            }

            this.productService.ProductOrder(product.Id, user.Id);
            return new RedirectResult("/");
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult Edit()
        {
            var productId = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.productService.GetProductById(productId);

            if (product == null)
            {
                return new RedirectResult("/");
            }

            this.Model.Data["EditProduct"] = new EditProduct
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                TypeToString = product.Type.ToString()
            };

            return this.View();
        }

        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Edit(CreateProductModel model)
        {
            var productId = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.productService.GetProductById(productId);

            if (product == null)
            {
                return new RedirectResult("/");
            }

            this.productService.EditProduct(productId, model.Name.UrlDecode(), model.Price, model.Description.UrlDecode(), Enum.Parse<Type>(model.Type));

            return new RedirectResult($"/Products/Details/?id={productId}");
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult Delete()
        {
            var productId = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.productService.GetProductById(productId);

            if (product == null)
            {
                return new RedirectResult("/");
            }

            this.Model.Data["DeleteProduct"] = new DeleteProduct
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                TypeToString = product.Type.ToString()
            };

            return this.View();
        }

        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Delete(int id)
        {
            var productId = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.productService.GetProductById(productId);

            if (product == null)
            {
                return new RedirectResult("/");
            }

            this.productService.DeleteProduct(productId);

            return new RedirectResult($"/");
        }
    }
}