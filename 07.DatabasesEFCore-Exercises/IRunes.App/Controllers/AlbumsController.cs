namespace IRunes.App.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class AlbumsController : Controller
    {
        public IHttpResponse Create()
        {
            return this.View("/Albums/create-album");
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            return new RedirectResult("/Albums/all");
        }
    }
}