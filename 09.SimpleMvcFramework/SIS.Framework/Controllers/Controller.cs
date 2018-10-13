﻿namespace SIS.Framework.Controllers
{
    using System.Runtime.CompilerServices;
    using ActionResults;
    using ActionResults.Contracts;
    using HTTP.Requests.Contracts;
    using Utilities;
    using Views;

    public abstract class Controller
    {
        protected Controller()
        {
        }

        public IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName] string caller = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);

            var fullyQualifiedName = ControllerUtilities.GetViewFullQualifiedName(controllerName, caller);

            var view = new View(fullyQualifiedName);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }
    }
}