namespace MishMash.App.Controllers
{
    using System.Collections.Generic;
    using Extensions;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Implementations;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Security;
    using ViewModels.Errors;
    using ViewModels.Users;

    public class UsersController : BaseController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            this.Model.Data["DisplayError"] = new DisplayError { Display = "none" };
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                this.Model.Data["DisplayError"] = new DisplayError { Display = "block", Message = "Passwords does not match!" };
                return this.View();
            }

            if (this.userService.IsUserExists(model.Username))
            {
                this.Model.Data["DisplayError"] = new DisplayError { Display = "block", Message = "Username already exists!" };
                return this.View();
            }

            var user = this.userService.AddUser(model.Username.UrlDecode(), model.Password, model.Email.UrlDecode());
            this.SignIn(new IdentityUser { Username = user.Username, Roles = new List<string> { user.Role.ToString() } });
            return new RedirectResult("/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            this.Model.Data["DisplayError"] = new DisplayError { Display = "none" };
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginViewModel model)
        {
            var user = this.userService.GetUserByUsernameAndPassword(model.Username.UrlDecode(), model.Password);

            if (user == null)
            {
                this.Model.Data["DisplayError"] = new DisplayError { Display = "block", Message = "Invalid username or password!" };
                return this.View();
            }

            this.SignIn(new IdentityUser { Username = user.Username, Roles = new List<string> { user.Role.ToString() } });
            return new RedirectResult("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();
            return new RedirectResult("/");
        }
    }
}