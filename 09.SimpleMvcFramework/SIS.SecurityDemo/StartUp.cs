namespace SIS.SecurityDemo
{
    using Controllers;
    using Framework.Api;
    using Framework.Services.Contracts;

    public class StartUp : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<HomeController, HomeController>();
        }
    }
}