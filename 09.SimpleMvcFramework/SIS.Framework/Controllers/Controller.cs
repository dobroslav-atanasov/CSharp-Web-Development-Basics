namespace SIS.Framework.Controllers
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using ActionResults;
    using ActionResults.Contracts;
    using HTTP.Cookies;
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using Models;
    using Security.Contracts;
    using Utilities;
    using Views;

    public abstract class Controller
    {
        private const string Auth = "auth";

        protected Controller()
        {
            this.Model = new ViewModel();
            this.ModelState = new Model();
            this.Response = new HttpResponse(HttpResponseStatusCode.Ok);
        }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        protected ViewModel Model { get; }

        public Model ModelState { get; }

        // New
        public IIdentity Identity => (IIdentity)this.Request.Session.GetParameter(Auth);

        private ViewEngine ViewEngine { get; }  = new ViewEngine();

        protected IViewable View([CallerMemberName] string actionName = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);
            string viewContent = null;

            try
            {
                viewContent = this.ViewEngine.GetViewContent(controllerName, actionName);
            }
            catch (FileNotFoundException ex)
            {
                this.Model.Data["Error"] = ex.Message;
                viewContent = this.ViewEngine.GetErrorContent();
            }

            var renderedContent = this.ViewEngine.RenderHtml(viewContent, this.Model.Data);
            return new ViewResult(new View(renderedContent));
        }

        // Old
        //protected IViewable View([CallerMemberName] string caller = "")
        //{
        //    var controllerName = ControllerUtilities.GetControllerName(this);

        //    var fullyQualifiedName = ControllerUtilities.GetViewFullQualifiedName(controllerName, caller);

        //    var view = new View(fullyQualifiedName, this.Model.Data);

        //    return new ViewResult(view);
        //}

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }

        // Old method
        protected bool IsLoggedIn()
        {
            return this.Request.Session.ContainsParameter("username");
        }

        // Old method
        protected void SignInUser(string username, string userCookie)
        {
            this.Request.Session.AddParameter("username", username);
            this.Response.Cookies.Add(new HttpCookie(HttpCookie.Auth, userCookie));
        }

        protected void SignIn(IIdentity auth)
        {
            this.Request.Session.AddParameter(Auth, auth);
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }
    }
}