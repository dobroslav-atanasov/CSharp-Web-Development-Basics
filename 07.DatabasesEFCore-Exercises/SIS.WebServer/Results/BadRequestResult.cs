namespace SIS.WebServer.Results
{
    using System.Text;
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;

    public class BadRequestResult : HttpResponse
    {
        public BadRequestResult(string content, HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            this.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}