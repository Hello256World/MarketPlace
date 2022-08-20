namespace MarketPlace.Web.PresentationExtension
{
    public static class HttpExtension
    {
        public static string GetUserIp(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
