namespace SIS.HTTP.Cookies
{
    using System;
    using Common;

    public class HttpCookie
    {
        private const int HttpCookieDefaultExpiresDays = 3;

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpiresDays)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));
            
            this.Key = key;
            this.Value = value;
            this.IsNew = true;
            this.Expires = DateTime.UtcNow.AddDays(expires);
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpiresDays)
            :this(key, value, expires)
        {
            this.IsNew = isNew;
        }

        public string Key { get; }

        public string Value { get; }

        public DateTime Expires { get; }

        public bool IsNew { get; }

        public override string ToString()
        {
            return $"{this.Key}={this.Value}; Expires={this.Expires.ToString("R")}";
        }
    }
}