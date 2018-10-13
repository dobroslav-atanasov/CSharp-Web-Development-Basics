namespace SIS.Framework
{
    using System.IO;
    using System.Linq;
    using HTTP.Common;
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using WebServer.Api;
    using WebServer.Results;
    using WebServer.Routing;

    public class HttpHandler : IHttpHandler
    {
        private ServerRoutingTable serverRoutingTable;

        public HttpHandler(ServerRoutingTable serverRoutingTable)
        {
            this.serverRoutingTable = serverRoutingTable;
        }

        public IHttpResponse Handle(IHttpRequest request)
        {
            var isResourceRequest = this.IsResourceRequest(request);

            if (isResourceRequest)
            {
                return this.HandleRequestResponse(request.Path);
            }

            if (!this.serverRoutingTable.Routes.ContainsKey(request.RequestMethod)
                || !this.serverRoutingTable.Routes[request.RequestMethod].ContainsKey(request.Path))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Routes[request.RequestMethod][request.Path].Invoke(request);
        }

        private IHttpResponse HandleRequestResponse(string httpRequestPath)
        {
            var startNameResourceIndex = httpRequestPath.LastIndexOf("/");
            var requestPathExtension = httpRequestPath.Substring(httpRequestPath.LastIndexOf("."));

            var resourceName = httpRequestPath.Substring(startNameResourceIndex);

            var resourcePath = $"../../../Resources/{requestPathExtension.Substring(1)}{resourceName}";

            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            var fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
        }

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            var requestPath = httpRequest.Path;
            if (requestPath.Contains("."))
            {
                var requestPathExtension = requestPath.Substring(requestPath.LastIndexOf("."));
                return GlobalConstans.Extensions.Contains(requestPathExtension);
            }
            return false;
        }
    }
}