namespace IRunes.App.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class TracksController : Controller
    {
        public IHttpResponse Create()
        {
            return this.View("/Tracks/create-track");
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            return new RedirectResult("/");
        }
    }
}