namespace NerdDinner.Controllers
{
    using System.Reflection;
    using System.Web.Mvc;

    public class DinnerActionDescriptor : ReflectedActionDescriptor
    {
        public DinnerActionDescriptor(MethodInfo methodInfo, string actionName, ControllerDescriptor controllerDescriptor)
            : base(methodInfo, actionName, controllerDescriptor)
        {
        }

        public override object Execute(ControllerContext controllerContext, System.Collections.Generic.IDictionary<string, object> parameters)
        {
            var actionResult = base.Execute(controllerContext, parameters);

            //TODO: You can do whatever you want here prior to returning the ActionResult to the MVC runtime.
            if (actionResult is ViewResult)
            {
                var viewResult = actionResult as ViewResult;
                var viewName = (string.IsNullOrEmpty(viewResult.ViewName)) ? base.ActionName : viewResult.ViewName;
                var prefix = string.Empty;

                if (controllerContext.IsiPhoneRequest())
                {
                    prefix = "Mobile/iPhone/";
                }
                else if (controllerContext.IsMobileRequest())
                {
                    prefix = "Mobile/";
                }

                viewResult.ViewName = prefix + viewName;
            }

            return actionResult;
        }
    }
}
