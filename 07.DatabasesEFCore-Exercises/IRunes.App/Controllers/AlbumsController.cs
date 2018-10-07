namespace IRunes.App.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;

    public class AlbumsController : Controller
    {
        public IHttpResponse All()
        {
            return this.View();
        }

        public IHttpResponse Create()
        {
            return this.View();
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            return this.View();
        }
    }
}