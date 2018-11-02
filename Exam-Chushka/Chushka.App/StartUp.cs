namespace Chushka.App
{
    using Controllers;
    using Services;
    using Services.Contracts;
    using SIS.Framework.Api;
    using SIS.Framework.Services;

    public class StartUp : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<HomeController, HomeController>();
            dependencyContainer.RegisterDependency<UsersController, UsersController>();
            dependencyContainer.RegisterDependency<ProductsController, ProductsController>();
            dependencyContainer.RegisterDependency<OrdersController, OrdersController>();

            dependencyContainer.RegisterDependency<IUserService, UserService>();
            dependencyContainer.RegisterDependency<IProductService, ProductService>();
            dependencyContainer.RegisterDependency<IOrderService, OrderService>();
        }
    }
}