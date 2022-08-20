using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Http
{
    public static class JsonResponseState
    {
        public static JsonResult SendResult(JsonResponseType type, string message, object data= null)
        {
            return new JsonResult(new
            {
                status = type.ToString(),
                message = message,
                data = data
            });
        }
    }
    public enum JsonResponseType
    {
        Success,
        Danger,
        Warning,
        Info
    }
}
