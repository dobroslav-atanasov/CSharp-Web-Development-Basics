namespace MishMash.App.Controllers
{
    using System.Collections.Generic;
    using Extensions;
    using Models.Enums;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Implementations;
    using SIS.Framework.Attributes.Method;
    using SIS.Framework.Controllers;
    using SIS.Framework.Security;
    using ViewModels.Errors;
    using ViewModels.Users;

    public class UsersController : Controller
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
            var username = model.Username.UrlDecode();
            var password = model.Password;
            var confirmPassword = model.ConfirmPassword;
            var email = model.Email.UrlDecode();
            var role = Role.User;

            if (password != confirmPassword)
            {
                this.Model.Data["DisplayError"] = new DisplayError { Display = "block", Message = "Passwords does not match!"};
                return this.View();
            }

            if (this.userService.IsUserExists(username))
            {
                this.Model.Data["DisplayError"] = new DisplayError { Display = "block", Message = "Username already exists!" };
                return this.View();
            }

            if (!this.userService.HaveUsers())
            {
                role = Role.Admin;
            }

            this.userService.AddUser(username, password, email, role);
            this.SignIn(new IdentityUser() { Username = username, Roles = new List<string> { role.ToString() } });
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
            var username = model.Username.UrlDecode();
            var password = model.Password;

            var user = this.userService.GetUser(username, password);

            if (user == null)
            {
                this.Model.Data["DisplayError"] = new DisplayError { Display = "block", Message = "Invalid username or password!" };
                return this.View();
            }

            this.SignIn(new IdentityUser() { Username = username, Roles = new List<string> { user.Role.ToString() } });
            return new RedirectResult("/");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.Model.Data["DisplayError"] = new DisplayError { Display = "none" };
            this.SignOut();
            return new RedirectResult("/");
        }
    }
}