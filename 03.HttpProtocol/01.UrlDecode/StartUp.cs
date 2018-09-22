namespace UrlDecode
{
    using System;
    using System.Net;

    public class StartUp
    {
        public static void Main()
        {
            var inputtedUrl = Console.ReadLine();
            var decodeUrl = WebUtility.UrlDecode(inputtedUrl);

            Console.WriteLine(decodeUrl);
        }
    }
}