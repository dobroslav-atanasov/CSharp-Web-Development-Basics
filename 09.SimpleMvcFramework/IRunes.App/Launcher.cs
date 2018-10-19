namespace IRunes.App
{
    using Services;
    using Services.Contracts;
    using SIS.Framework;
    using SIS.Framework.Routers;
    using SIS.Framework.Services;
    using SIS.Framework.Services.Contracts;
    using SIS.WebServer;

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

            services.RegisterDependency<IUserService, UserService>();

            return services;
        }
    }
}