namespace Torshia.App.Controllers
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
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return this.View();
            }

            var username = model.Username.UrlDecode();
            var password = model.Password.UrlDecode();
            var email = model.Email.UrlDecode();
            var role = Role.User;

            if (!this.userService.AreThereUsers())
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
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserViewModel model)
        {
            var username = model.Username.UrlDecode();
            var password = model.Password.UrlDecode();

            var user = this.userService.GetUser(username, password);

            if (user != null)
            {
                this.SignIn(new IdentityUser() { Username = username, Roles = new List<string> { user.Role.ToString() } });
                return new RedirectResult("/");
            }

            return new RedirectResult("/");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.SignOut();
            return new RedirectResult("/");
        }
    }
}