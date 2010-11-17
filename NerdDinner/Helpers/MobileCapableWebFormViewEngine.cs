using System;
using System.Web.Mvc;

namespace NerdDinner
{
    public class MobileCapableWebFormViewEngine : IViewEngine
    {
        private readonly WebFormViewEngine mobileViewEngine;
        private readonly WebFormViewEngine desktopViewEngine;

        public MobileCapableWebFormViewEngine()
        {
            mobileViewEngine = new WebFormViewEngine { ViewLocationCache = new DefaultViewLocationCache() };
            desktopViewEngine = new WebFormViewEngine { ViewLocationCache = new DefaultViewLocationCache() };
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var request = controllerContext.HttpContext.Request;

            //This could be replaced with a switch statement as other advanced / device specific views are created
            if (UserAgentIs(controllerContext, "iPhone"))
            {
                return mobileViewEngine.FindPartialView(controllerContext, "Mobile/iPhone/" + partialViewName, useCache);
            }

            var isMobileDevice = request.Browser.IsMobileDevice;

            //HACK: This could've been handeled differently, but it works.
            var isAndroid = UserAgentIs(controllerContext, "Android");
            
            if (isMobileDevice || isAndroid) 
            {
                return mobileViewEngine.FindPartialView(controllerContext, "Mobile/" + partialViewName, useCache)
                    ?? mobileViewEngine.FindPartialView(controllerContext, partialViewName, useCache);
            }

            return desktopViewEngine.FindPartialView(controllerContext, partialViewName, useCache);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var request = controllerContext.HttpContext.Request;

            //This could be replaced with a switch statement as other advanced / device specific views are created
            if (UserAgentIs(controllerContext, "iPhone"))
            {
                return mobileViewEngine.FindView(controllerContext, "Mobile/iPhone/" + viewName, masterName, useCache);
            }

            var isMobileDevice = request.Browser.IsMobileDevice;

            //HACK: This could've been handeled differently, but it works.
            var isAndroid = UserAgentIs(controllerContext, "Android");
            
            if (isMobileDevice || isAndroid)
            {
                return mobileViewEngine.FindView(controllerContext, "Mobile/" + viewName, masterName, useCache)
                    ?? mobileViewEngine.FindView(controllerContext, viewName, masterName, useCache);
            }

            return desktopViewEngine.FindView(controllerContext, viewName, masterName, useCache);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            mobileViewEngine.ReleaseView(controllerContext, view);
            desktopViewEngine.ReleaseView(controllerContext, view);
        }

        public bool UserAgentIs(ControllerContext controllerContext, string userAgentToTest)
        {
            return (controllerContext.HttpContext.Request.UserAgent.IndexOf(userAgentToTest,
                            StringComparison.OrdinalIgnoreCase) > 0);
        }
    }
}
