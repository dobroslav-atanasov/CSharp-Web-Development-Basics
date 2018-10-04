namespace IRunes.App
{
    using Controllers;
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;

    public class StartUp
    {
        public static void Main()
        {
            var serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index();
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/login"] = request => new UserController().Login();
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/login"] = request => new UserController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/register"] = request => new UserController().Register();
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/register"] = request => new UserController().Register(request);

            var server = new Server(8080, serverRoutingTable);
            server.Run();
        }
    }
}