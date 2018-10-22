namespace SIS.Framework
{
    using Api.Contracts;
    using Routers;
    using Services;
    using WebServer;

    public static class WebHost
    {
        private const int Port = 8080;

        public static void Start(IMvcApplication application)
        {
            var container = new DependencyContainer();
            application.ConfigureServices(container);

            var handlers = new HttpHandlerContextRouter(new ControllerRouter(container), new ResourceRouter());
            application.Configure();

            var server = new Server(Port, handlers);
            server.Run();
        }
    }
}