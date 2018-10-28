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
        public IActionResult Register(UserRegisterViewModel model)
        {
            var username = model.Username.UrlDecode();
            var password = model.Password;
            var confirmPassword = model.ConfirmPassword;
            var email = model.Email.UrlDecode();
            var role = Role.User;

            if (password != confirmPassword)
            {
                return this.View();
            }

            if (this.userService.IsUserExists(username))
            {
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
            return this.View();
        }
    }
}