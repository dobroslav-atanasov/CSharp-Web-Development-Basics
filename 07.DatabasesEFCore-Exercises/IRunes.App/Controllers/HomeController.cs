namespace IRunes.App.Controllers
{
    using System.IO;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            var content = File.ReadAllText($"{Directory.GetCurrentDirectory()}/Views/index.html");

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}