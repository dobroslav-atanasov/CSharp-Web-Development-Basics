﻿namespace IRunes.App
{
    using Controllers;
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;

    public class StartUp
    {
        public static void Main()
        {
            var serverRoutingTable = new ServerRoutingTable();

            // HomeController
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/home/index"] = request => new RedirectResult("/");
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index();

            // UserController
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/login"] = request => new UsersController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/login"] = request => new UsersController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/register"] = request => new UsersController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/register"] = request => new UsersController().DoRegister(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/logout"] = request => new UsersController().Logout(request);

            // AlbumController
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/create-album"] = request => new AlbumsController().Create();
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/create-album"] = request => new AlbumsController().Create(request);

            // TracksController
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/create-track"] = request => new TracksController().Create();
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/create-track"] = request => new TracksController().Create(request);

            // add in SIS.HTTP in cookies delete method

            var server = new Server(8080, serverRoutingTable);
            server.Run();
        }
    }
}