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
            return string.IsNullOrEmpty(queryString) && queryParameters.Length >= 1;
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            var isValidRequestMethod = Enum.TryParse<HttpRequestMethod>(requestLine[0], true, out var method);
            if (!isValidRequestMethod)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = method;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            // TODO create validation
            this.Url = requestLine[1];
        }

        private void ParseRequestPath()
        {
            var parts = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            this.Path = parts[0];
        }

        private void ParseHeaders(string[] requestContent)
        {
            if (!requestContent.Any())
            {
                throw new BadRequestException();
            }

            foreach (var line in requestContent)
            {
                if (string.IsNullOrEmpty(line))
                {
                    return; 
                }

                var parts = line.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                var header = new HttpHeader(parts[0], parts[1]);
                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsHeader(GlobalConstans.HostHeaderKey))
            {
                throw new BadRequestException();
            }
        }

        private void ParseQueryParameters()
        {
            var query = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            var queryString = query[1];
            if (query.Contains("#"))
            {
                queryString = query[1].Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            if (string.IsNullOrEmpty(queryString))
            {
                throw new BadRequestException();
            }

            var queryParameters = queryString.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            this.IsValidRequestQueryString(queryString, queryParameters);

            foreach (var pair in queryParameters)
            {
                var parts = pair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                this.QueryData.Add(parts[0], parts[1]);
            }
        }

        private void ParseFormDataParameters(string formData)
        {
            if (this.RequestMethod == HttpRequestMethod.Get)
            {
                return;
            }

            var requestBodyParts = formData.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in requestBodyParts)
            {
                var parts = pair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                this.FormData.Add(parts[0], parts[1]);
            }
        }

        private void ParseRequestParameters(string formData)
        {
            this.ParseQueryParameters();
            this.ParseFormDataParameters(formData);
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