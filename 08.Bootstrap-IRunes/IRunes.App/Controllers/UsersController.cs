namespace IRunes.App.Controllers
{
    using Extensions;
    using Services;
    using Services.Contracts;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class UsersController : Controller
    {
        private const string InvalidUsernameOrPassword = "<h2>Invalid username, email or password!</h2>";
        private const string InvalidUsernameOrPasswordLength = "<h2>Invalid user data!</h2>";
        private const string UsernameAlreadyExists = "<h2>Username already exists!</h2>";

        private readonly IUserService userService;
        private readonly IHashService hashService;

        public UsersController()
        {
            this.userService = new UserService();
            this.hashService = new HashService();
        }

        public IHttpResponse Login()
        {
            return this.NewView("login", this.ViewBag);
            //this.SetViewBagData();
            //return this.View();
        }
        
        public IHttpResponse Login(IHttpRequest request)
        {
            var usernameOrEmail = request.FormData["usernameOrEmail"].ToString().UrlDecode();
            var password = request.FormData["password"].ToString().UrlDecode();

            var hashPassword = this.hashService.Hash(password);
            var user = this.userService.GetUserWithUsernameOrEmail(usernameOrEmail, hashPassword);

            if (user == null)
            {
                this.ViewBag["Error"] = InvalidUsernameOrPassword;
                return this.NewView("error", this.ViewBag);
                //this.ApplyError(InvalidUsernameOrPassword);
                //return this.View();
            }

            var response = new RedirectResult("/");
            this.SignInUser(usernameOrEmail, request, response);
            return response;
        }

        public IHttpResponse Register()
        {
            return this.NewView("register", this.ViewBag);
            //this.SetViewBagData();
            //return this.View();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().UrlDecode();
            var password = request.FormData["password"].ToString().UrlDecode();
            var confirmPassword = request.FormData["confirmPassword"].ToString().UrlDecode();
            var email = request.FormData["email"].ToString().UrlDecode();

            if (username.Length < 4 || password.Length < 4 || password != confirmPassword)
            {
                this.ViewBag["Error"] = InvalidUsernameOrPasswordLength;
                return this.NewView("error", this.ViewBag);
                //this.ApplyError(InvalidUsernameOrPasswordLength);
                //return this.View();
            }

            if (this.userService.ContainsUser(username))
            {
                this.ViewBag["Error"] = UsernameAlreadyExists;
                return this.NewView("error", this.ViewBag);
                //this.ApplyError(UsernameAlreadyExists);
                //return this.View();
            }

            var hashPassword = this.hashService.Hash(password);
            this.userService.AddUser(username, hashPassword, email);

            var response = new RedirectResult("/");
            this.SignInUser(username, request, response);
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Session.ContainsParameter("username"))
            {
                return new RedirectResult("/");
            }

            request.Session.ClearParameters();
            return new RedirectResult("/");
        }
    }
}