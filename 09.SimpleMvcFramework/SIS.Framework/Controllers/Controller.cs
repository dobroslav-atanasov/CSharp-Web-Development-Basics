namespace SIS.Framework.Controllers
{
    using System.Runtime.CompilerServices;
    using ActionResults;
    using ActionResults.Contracts;
    using HTTP.Cookies;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using Models;
    using Services;
    using Services.Contracts;
    using Utilities;
    using Views;

    public abstract class Controller
    {
        private readonly IUserCookieService userCookieService;
        
        protected Controller()
        {
            this.Model = new ViewModel();
            this.ModelState = new Model();
            this.userCookieService = new UserCookieService();
        }

        public IHttpRequest Request { get; set; }

        protected ViewModel Model { get; }

        public Model ModelState { get; }

        private HttpCookie Cookie { get; set; }

        protected IViewable View([CallerMemberName] string caller = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);

            var fullyQualifiedName = ControllerUtilities.GetViewFullQualifiedName(controllerName, caller);

            var view = new View(fullyQualifiedName, this.Model.Data);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }

        protected bool IsLoggedIn()
        {
            return this.Request.Session.ContainsParameter("username");
        }

        protected void SignInUser(string username)
        {
            this.Cookie = this.userCookieService.GetUserCookie(username);
        }
    }
}