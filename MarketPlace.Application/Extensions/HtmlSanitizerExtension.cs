using Ganss.XSS;

namespace MarketPlace.Application.Extensions
{
    public static class HtmlSanitizerExtension
    {
        public static string Sanitize(this string text)
        {
            var sanitizer = new HtmlSanitizer();

            sanitizer.KeepChildNodes = true;

            sanitizer.AllowDataAttributes = true;

            var sanitized = sanitizer.Sanitize(text);

            return sanitized;
        }
    }
}
