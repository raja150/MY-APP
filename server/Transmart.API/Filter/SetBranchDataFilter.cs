using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TranSmart.API.Filter
{
    public class SetBranchDataFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
			throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        { 
            if (context.ActionArguments.TryGetValue("returnUrl", out object value))
            {
                // NOTE: this assumes all your controllers derive from Controller.
                // If they don't, you'll need to set the value in OnActionExecuted instead
                // or use an IAsyncActionFilter
                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ReturnUrl"] = value.ToString();
                }
            }
        }
    }
}
