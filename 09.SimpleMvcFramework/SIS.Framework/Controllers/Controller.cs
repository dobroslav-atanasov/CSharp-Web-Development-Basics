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

        private ViewEngine ViewEngine { get; } = new ViewEngine();

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

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }

        protected void SignIn(IIdentity auth)
        {
            this.Request.Session.AddParameter(Auth, auth);
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }

        public IIdentity Identity()
        {
            if (this.Request.Session.ContainsParameter(Auth))
            {
                return (IIdentity)this.Request.Session.GetParameter(Auth);
            }

            return null;
        }
    }
}