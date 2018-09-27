namespace SIS.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Contracts;
    using Enums;
    using Exceptions;
    using Headers;
    using Headers.Contracts;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private bool IsValidRequestLine(string[] requestLine)
        {
            return requestLine.Length == 3 && requestLine[2] == GlobalConstans.HttpOneProtocolFragment;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            throw new NotImplementedException();
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            throw new NotImplementedException();
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            throw new NotImplementedException();
        }

        private void ParseRequestPath()
        {
            throw new NotImplementedException();
        }

        private void ParseHeaders(string[] requestContent)
        {
            throw new NotImplementedException();
        }

        private void ParseQueryParameters()
        {
            throw new NotImplementedException();
        }

        private void ParseFormDataParameters(string formData)
        {
            throw new NotImplementedException();
        }

        private void ParseRequestParameters(string formData)
        {
            throw new NotImplementedException();
        }

        private void ParseRequest(string requestString)
        {
            var requestParts = requestString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var requestLine = requestParts[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();
            this.ParseHeaders(requestParts.Skip(1).ToArray());
            this.ParseRequestParameters(requestParts[requestParts.Length - 1]);
        }
    }
}