namespace Torshia.App.Extensions
{
    using System.Net;

    public static class StringExtension
    {
        public static string UrlDecode(this string text)
        {
            return WebUtility.UrlDecode(text);
        }
    }
}