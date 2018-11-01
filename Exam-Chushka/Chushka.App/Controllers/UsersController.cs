namespace Chushka.App.Controllers
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
        public IActionResult Register(RegisterUserModel model)
        {
            var username = model.Username.UrlDecode();
            var password = model.Password;
            var confirmPassword = model.ConfirmPassword;
            var fullName = model.FullName.UrlDecode();
            var email = model.Email.UrlDecode();

            if (password != confirmPassword)
            {
                this.Model.Data["DisplayError"] = new DisplayError { Display = "block", Message = "Passwords does not match!" };
                return this.View();
            }

            if (this.userService.IsUserExists(username))
            {
                this.Model.Data["DisplayError"] = new DisplayError { Display = "block", Message = "Username already exists!" };
                return this.View();
            }

            this.userService.AddUser(username, password, fullName, email);
            var user = this.userService.GetUserByUsername(username);
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
        public IActionResult Login(LoginUserModel model)
        {
            var username = model.Username.UrlDecode();
            var password = model.Password;

            var user = this.userService.GetUserByUsernameAndPassword(username, password);
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