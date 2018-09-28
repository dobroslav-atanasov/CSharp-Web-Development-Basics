namespace SIS.Demo
{
    using HTTP.Enums;
    using HTTP.Responses.Contracts;
    using WebServer.Results;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            var content = "<h1>Hello, World!<h1>";

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}