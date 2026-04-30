using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranSmart.API
{
    public class LoginEmployeeActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _ = context.Result;
        }

        
        public void OnActionExecuting(ActionExecutingContext context)
        {
           
            _= context.ActionArguments;

            if (context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "eid").Value == string.Empty) return;
            var employeeId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "eid").Value;
           
            if (context.HttpContext.Request.Path.ToString().Contains("Paginate"))
            {//context.ActionDescriptor.Pro
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(
                                   new
                                   {
                                       controller =context.Controller.GetType().Name.Replace("Controller",""),
                                       action = "SelfServiceSearch"
                                   }));
            }
            if (context.HttpContext.Request.Path.ToString().Contains("SelfServiceSearch"))
            {
                var model = context.ActionArguments["baseSearch"] as TranSmart.Domain.Models.BaseSearch;
                if (model != null)
                {
                    model.RefId = Guid.Parse(employeeId);
                    model.Size = 30;
                }
            }

            if (string.Equals(context.HttpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase)
                || string.Equals(context.HttpContext.Request.Method, "PUT", StringComparison.OrdinalIgnoreCase))
            {
                switch (context.Controller.GetType().Name.ToLower())
                {
                    case "applyleavescontroller":
                        var model = context.ActionArguments["model"] as TranSmart.Domain.Models.SelfService.ApplyLeavesModel;
                        if (model != null)
                        {
                            model.EmployeeId = Guid.Parse(employeeId);
                        }
                        break;
                    case "applywfhcontroller":
                        var modelWFH = context.ActionArguments["model"] as TranSmart.Domain.Models.Leave.ApplyWfhModel;
                        if (modelWFH != null)
                        {
                            modelWFH.EmployeeId = Guid.Parse(employeeId);
                        }
                        break;
                    case "raiseticketcontroller":
                        var modelRaiseTic = context.ActionArguments["model"] as TranSmart.Domain.Models.SelfService.RaiseTicketModel;
                        if (modelRaiseTic != null)
                        {
                            modelRaiseTic.EmployeeId = Guid.Parse(employeeId);
                        }
                        break; 
                    case "applycompensatoryworkingdaycontroller":
                        var modelComp = context.ActionArguments["model"] as TranSmart.Domain.Models.SelfService.ApplyCompensatoryWorkingDayModel;
                        if (modelComp != null)
                        {
                            modelComp.EmployeeId = Guid.Parse(employeeId);
                        }
                        break;
                    case "applyclientvisitscontroller":
                        var modelVisists = context.ActionArguments["model"] as TranSmart.Domain.Models.SelfService.ApplyClientVisitsModel;
                        if (modelVisists != null)
                        {
                            modelVisists.EmployeeId = Guid.Parse(employeeId);
                        }
                        break;
                }
            }
        }
    }
}
