namespace IRunes.App.Controllers
{
    using SIS.HTTP.Responses.Contracts;

    public class HomeController : Controller
    {
        public IHttpResponse Index()
        {
            return this.View();
        }
    }
}