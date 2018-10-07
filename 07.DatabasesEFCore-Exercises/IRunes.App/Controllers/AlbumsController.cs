namespace IRunes.App.Controllers
{
    using Services;
    using Services.Contracts;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class AlbumsController : Controller
    {
        private readonly IUserCookieService userCookieService;

        public AlbumsController()
        {
            this.userCookieService = new UserCookieServer();
        }

        public IHttpResponse Create()
        {
            return this.View("/Albums/create-album");
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-irunes"))
            {
                return new RedirectResult("/login");
            }

            var username = this.userCookieService.GetUserData(request.Cookies.GetCookie(".auth-irunes").Value);

            return new RedirectResult("/Albums/all");
        }
    }
}