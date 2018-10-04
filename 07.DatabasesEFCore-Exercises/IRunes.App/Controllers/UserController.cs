namespace IRunes.App.Controllers
{
    using System.IO;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class UserController
    {
        public IHttpResponse Login()
        {
            var content = File.ReadAllText($"{Directory.GetCurrentDirectory()}/Views/login.html");

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();

            return new RedirectResult("/");
        }
    }
}