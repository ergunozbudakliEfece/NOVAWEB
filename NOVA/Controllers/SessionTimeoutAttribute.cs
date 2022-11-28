using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NOVA.Controllers
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session.SessionID != null)
            {
                
            }
            else
            {
                filterContext.Result = new RedirectResult("/Login/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}