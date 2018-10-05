namespace IRunes.App.Controllers
{
    using System.IO;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public abstract class Controller
    {
        protected IHttpResponse View(string viewName)
        {
            //var content = File.ReadAllText($"{Directory.GetCurrentDirectory()}/Views/{viewName}.html");
            var content = File.ReadAllText($"../../../Views{viewName}.html");
            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        protected IHttpResponse BadRequestError(string message)
        {
            return new HtmlResult($"<h1>{message}<h1>", HttpResponseStatusCode.BadRequest);
        }
    }
}