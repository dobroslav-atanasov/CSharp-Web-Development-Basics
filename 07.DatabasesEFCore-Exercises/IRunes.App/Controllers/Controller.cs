namespace IRunes.App.Controllers
{
    using System.IO;
    using System.Runtime.CompilerServices;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public abstract class Controller
    {
        private const string ControllerSuffixName = "Controller";

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            var path = $"../../../Views/{this.GetController()}/{viewName}.html";

            if (!File.Exists(path))
            {
                return new BadRequestResult(HttpResponseStatusCode.NotFound);
            }

            var content = File.ReadAllText(path);
            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        private string GetController()
        {
            var controllerName = this.GetType().Name.Replace(ControllerSuffixName, string.Empty);
            return controllerName;
        }
    }
}