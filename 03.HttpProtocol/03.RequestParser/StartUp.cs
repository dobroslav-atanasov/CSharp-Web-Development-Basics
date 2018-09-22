namespace RequestParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    public class StartUp
    {
        private const string EndCommand = "end";

        public static void Main()
        {
            var inputtedUrls = new List<string>();
            var input = Console.ReadLine();

            while (input.ToLower() != EndCommand)
            {
                inputtedUrls.Add(input);
                input = Console.ReadLine();
            }

            var requestParts = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            var method = requestParts[0];
            var path = requestParts[1];
            var protocol = requestParts[2];

            var request = $"{path}/{method.ToLower()}";

            if (inputtedUrls.Contains(request))
            {
                var status = HttpStatusCode.OK;
                PrintResponse(protocol, status);
            }
            else
            {
                var status = HttpStatusCode.NotFound;
                PrintResponse(protocol, status);
            }
        }

        private static void PrintResponse(string protocol, HttpStatusCode status)
        {
            var sb = new StringBuilder();
            sb.AppendLine()
                .AppendLine($"{protocol} {(int)status} {status}")
                .AppendLine($"Content-Length: {status.ToString().Length}")
                .AppendLine($"Content-Type: taxt/plain")
                .AppendLine()
                .Append($"{status}");

            Console.WriteLine(sb.ToString());
        }
    }
}