using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelperAdmin
{
    public class BindAdminParameters : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {

            var culture = System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            int userId = 0;

            if (filterContext.RequestContext.HttpContext.Session["adminId"] != null)
            {
                string strUserId=filterContext.RequestContext.HttpContext.Session["adminId"].ToString();
               int.TryParse(strUserId, out userId);
            }

            filterContext.Controller.ViewBag.adminVariable = ((IAdminVariable)filterContext.Controller).getAdminVariable(userId);

            base.OnActionExecuting(filterContext);
        }

       
    }
}