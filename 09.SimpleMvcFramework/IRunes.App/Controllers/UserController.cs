namespace IRunes.App.Controllers
{
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Controllers;
    using SIS.Framework.Security;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Extensions;
    using ViewModels;

    public class UserController : Controller
    {
        private const string None = "none";
        private const string Inline = "inline";
        private const string InvalidRegistrationData = "Invalid registration data!";
        private const string UsernameExists = "Username exists!";
        private const string InvalidLoginData = "Invalid username or password!";

        private readonly IUserService userService;
        private readonly IHashService hashService;

        public UserController(IUserService userService, IHashService hashService)
        {
            this.userService = userService;
            this.hashService = hashService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            this.Model.Data["ErrorViewModel"] = new ErrorViewModel
            {
                DisplayError = None
            };

            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterViewModel model)
        {
            var username = model.Username.UrlDecode();
            var password = model.Password.UrlDecode();
            var confirmPassword = model.Confirmpassword.UrlDecode();
            var email = model.Email.UrlDecode();

            if (!this.IsValidModel(username, password, confirmPassword))
            {
                this.Model.Data["ErrorViewModel"] = new ErrorViewModel
                {
                    DisplayError = Inline,
                    ErrorMessage = InvalidRegistrationData
                };
                return this.View();
            }

            if (this.userService.ContainsUser(username))
            {
                this.Model.Data["ErrorViewModel"] = new ErrorViewModel
                {
                    DisplayError = Inline,
                    ErrorMessage = UsernameExists
                };
                return this.View();
            }

            var hashPassword = this.hashService.Hash(password);
            this.userService.AddUser(username, hashPassword, email);

            this.SignIn(new IdentityUser() { Username = username });
            return new RedirectResult("/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            this.Model.Data["ErrorViewModel"] = new ErrorViewModel
            {
                DisplayError = None
            };

            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginViewModel model)
        {
            var username = model.Username.UrlDecode();
            var password = model.Password.UrlDecode();

            var hashPassword = this.hashService.Hash(password);
            var user = this.userService.GetUserWithUsernameOrEmail(username, hashPassword);

            if (user == null)
            {
                this.Model.Data["ErrorViewModel"] = new ErrorViewModel
                {
                    DisplayError = Inline,
                    ErrorMessage = InvalidLoginData
                };
                return this.View();
            }

            this.SignIn(new IdentityUser { Username = username });
            return new RedirectResult("/");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.SignOut();
            return new RedirectResult("/");
        }

        private bool IsValidModel(string username, string password, string confirmPassword)
        {
            return username.Length >= 3 && password.Length >= 3 && password == confirmPassword;
        }
    }
}