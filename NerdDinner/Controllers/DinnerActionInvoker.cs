namespace NerdDinner.Controllers
{
    using System.Web.Mvc;

    public class DinnerActionInvoker : ControllerActionInvoker
    {
        protected override ActionDescriptor FindAction(ControllerContext controllerContext,
                                                       ControllerDescriptor controllerDescriptor, string actionName)
        {
            // Find the base action
            var foundAction = base.FindAction(controllerContext, controllerDescriptor, actionName);

            if (foundAction is ReflectedActionDescriptor)
            {
                var reflectedDescriptor = foundAction as ReflectedActionDescriptor;
                foundAction = new DinnerActionDescriptor(reflectedDescriptor.MethodInfo, actionName, controllerDescriptor);
            }

            return foundAction;
        }
    }
}