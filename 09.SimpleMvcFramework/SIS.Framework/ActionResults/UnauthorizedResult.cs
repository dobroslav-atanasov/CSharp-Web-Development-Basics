namespace SIS.Framework.ActionResults
{
    using System.Text;
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;

    public class UnauthorizedResult : HttpResponse
    {
        private const string DefaultErrorHandling = "<h1>You have not permission to access this functionality.</h1>";

        public UnauthorizedResult()
            :base(HttpResponseStatusCode.Unauthorized)
        {
            this.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html"));
            this.Content = Encoding.UTF8.GetBytes(DefaultErrorHandling);
        }
    }
}