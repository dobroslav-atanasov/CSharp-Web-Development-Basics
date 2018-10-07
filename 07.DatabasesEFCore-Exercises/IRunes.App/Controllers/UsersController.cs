﻿namespace IRunes.App.Controllers
{
    using Services;
    using Services.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IHashService hashService;

        public UsersController()
        {
            this.userService = new UserService();
            this.hashService = new HashService();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View();
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();

            var hashPassword = this.hashService.Hash(password);
            var user = this.userService.GetUser(username, hashPassword);

            if (user == null)
            {
                return new RedirectResult("/users/login");
            }

            var response = new RedirectResult("/");
            this.SignInUser(username, request, response);

            return new RedirectResult("/");

            //var username = request.FormData["username"].ToString();
            //var password = request.FormData["password"].ToString();

            //var hashPassword = this.hashService.Hash(password);

            //if (!this.userService.ContainsUser(username))
            //{
            //    //return this.BadRequestError($"User {username} does not exist!");
            //}

            //var user = this.userService.GetUser(username, hashPassword);
            //if (user == null)
            //{
            //    //return this.BadRequestError($"Invalid username or password!");
            //}

            //var response = new RedirectResult("/");
            //var cookieContent = this.userCookieService.GetUserCookie(username);
            //var cookie = new HttpCookie(".auth-irunes", cookieContent, 3);
            //response.AddCookie(cookie);

            //return response;
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View();
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();
            var email = request.FormData["email"].ToString();

            // TODO Validation

            var hashPassword = this.hashService.Hash(password);
            this.userService.AddUser(username, hashPassword, email);

            var response = new RedirectResult("/");
            this.SignInUser(username, request, response);

            return response;


            //var username = request.FormData["username"].ToString();
            //var password = request.FormData["password"].ToString();
            //var confirmPassword = request.FormData["confirmPassword"].ToString();
            //var email = request.FormData["email"].ToString();

            //if (username.Length < 4 || password.Length < 4)
            //{
            //    //return this.BadRequestError("Invalid user data!");
            //}

            //if (password != confirmPassword)
            //{
            //    //return this.BadRequestError("Passwords are different!");
            //}

            //if (this.userService.ContainsUser(username))
            //{
            //    //return this.BadRequestError($"User {username} already exists!");
            //}

            //var hashedPassword = this.hashService.Hash(password);

            //this.userService.AddUser(username, hashedPassword, email);
            //var response = new RedirectResult("/");
            //var cookieContent = this.userCookieService.GetUserCookie(username);
            //var cookie = new HttpCookie(".auth-irunes",cookieContent, 3);
            //response.AddCookie(cookie);

            //return response;
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