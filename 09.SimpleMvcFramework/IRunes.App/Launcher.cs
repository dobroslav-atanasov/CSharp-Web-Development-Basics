namespace IRunes.App
{
    using SIS.Framework;
    using SIS.Framework.Routers;
    using SIS.WebServer;

    public class Launcher
    {
        public static void Main()
        {
            var handlers = new HttpHandlerContextRouter(new ControllerRouter(), new ResourceRouter());
            var server = new Server(8080, handlers);

            MvcEngine.Run(server);
        }
    }
}