namespace SIS.FrameworkDemo
{
    using Framework;
    using Framework.Routers;
    using WebServer;

    public class Laucher
    {
        public static void Main()
        {
            var server = new Server(8080, new ControllerRouter());

            MvcEngine.Run(server);
        }
    }
}