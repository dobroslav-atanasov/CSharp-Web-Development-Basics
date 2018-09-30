namespace SIS.Demo
{
    using System.IO;
    using HTTP.Enums;
    using HTTP.Responses.Contracts;
    using WebServer.Results;

    public class HomeController
    {
        private const string FolderPath = "../../../Resources/";

        public IHttpResponse Index()
        {
            var content = File.ReadAllText($"{FolderPath}index.html");

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}