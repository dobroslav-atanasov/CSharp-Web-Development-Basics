namespace SIS.FrameworkDemo
{
    using Controllers;
    using Framework;
    using Framework.Routers;
    using Framework.Services;
    using Framework.Services.Contracts;
    using WebServer;

    public class Launcher
    {
        public static void Main()
        {
            var services = ConfigureServices();

            var handlers = new HttpHandlerContextRouter(new ControllerRouter(services), new ResourceRouter());
            var server = new Server(8080, handlers);

            MvcEngine.Run(server);
        }

        private static IDependencyContainer ConfigureServices()
        {
            var services = new DependencyContainer();

            services.RegisterDependency<HomeController, HomeController>();

            return services;
        }
    }
}