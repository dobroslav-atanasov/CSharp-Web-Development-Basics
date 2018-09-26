namespace SIS.HTTP.Extensions
{
    public class StringExtensions
    {
        public string Capitalize(string message)
        {
            var firstLetter = message[0].ToString().ToUpper();
            var allOther = message.Substring(1).ToLower();

            return $"{firstLetter}{allOther}";
        }
    }
}