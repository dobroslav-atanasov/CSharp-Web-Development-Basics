namespace ValidateUrl
{
    using System;
    using System.Net;

    public class StartUp
    {
        public static void Main()
        {
            var inputtedUrl = WebUtility.UrlDecode(Console.ReadLine());
            var uri = new Uri(inputtedUrl);

            if (uri.Port < 0 || (uri.Scheme == "http" && uri.Port == 443) || (uri.Scheme == "https" && uri.Port == 80))
            {
                Console.WriteLine("Invalid URL");
                return;
            }

            Console.WriteLine($"Protocol: {uri.Scheme}");
            Console.WriteLine($"Host: {uri.Host}");
            Console.WriteLine($"Port: {uri.Port}");
            Console.WriteLine($"Path: {uri.LocalPath}");

            if (!string.IsNullOrEmpty(uri.Query))
            {
                var query = uri.Query.Substring(1);
                Console.WriteLine($"Query: {query}");
            }
            if (!string.IsNullOrEmpty(uri.Fragment))
            {
                var fragment = uri.Fragment.Substring(1);
                Console.WriteLine($"Fragment: {fragment}");
            }
        }
    }
}