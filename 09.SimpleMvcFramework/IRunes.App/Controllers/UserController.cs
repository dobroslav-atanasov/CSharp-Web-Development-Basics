namespace IRunes.App.Controllers
{
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Controllers;
    using SIS.HTTP.Extensions;
    using ViewModels;

    public class UserController : Controller
    {
        private const string None = "none";
        private const string Inline = "inline";
        private const string DisplayError = "DisplayError";
        private const string ErrorMessage = "ErrorMessage";
        private const string InvalidRegistrationData = "Invalid registration data!";
        private const string UsernameExists = "Username exists!";
        
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
            this.Model.Data[DisplayError] = None;
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
                this.Model.Data[DisplayError] = Inline;
                this.Model.Data[ErrorMessage] = InvalidRegistrationData;
                return this.View();
            }

            if (this.userService.ContainsUser(username))
            {
                this.Model.Data[DisplayError] = Inline;
                this.Model.Data[ErrorMessage] = UsernameExists;
                return this.View();
            }

            var hashPassword = this.hashService.Hash(password);
            this.userService.AddUser(username, hashPassword, email);

            this.SignInUser(username);
            return new RedirectResult("/");
        }

        private bool IsValidModel(string username, string password, string confirmPassword)
        {
            return username.Length >= 3 && password.Length >= 3 && password == confirmPassword;
        }
    }
}