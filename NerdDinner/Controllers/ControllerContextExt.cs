namespace NerdDinner.Controllers {
    using System;
    using System.Web.Mvc;

    public static class ControllerContextExt
    {
        public static bool IsiPhoneRequest(this ControllerContext context)
        {
            return IsUserAgent(context, "iPhone");
        }
        public static bool IsMobileRequest(this ControllerContext context)
        {
            var request = context.HttpContext.Request;
            var isMobile = request.Browser.IsMobileDevice;
            return isMobile || IsUserAgent(context, "Android");
        }

        public static bool IsUserAgent(this ControllerContext context, string userAgent)
        {
            var request = context.HttpContext.Request;
            return request.UserAgent.IndexOf(userAgent, StringComparison.OrdinalIgnoreCase) > 0;
        }
    }
}