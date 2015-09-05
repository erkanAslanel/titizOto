
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HelperSite.DbController;

namespace HelperSite.Attribute
{
    public class langBind : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            DbWithController itemController = (DbWithController)filterContext.Controller;

            var trCulture = CultureInfo.CreateSpecificCulture("tr-TR");
            var enCulture = CultureInfo.CreateSpecificCulture("en-US"); 

            if (filterContext.RouteData.Values["lang"] != null)
            {
                string langText = filterContext.RouteData.Values["lang"].ToString().ToLower();

              

                switch (langText)
                {
                    case "tr":
                        itemController.langCode = "tr";
                        itemController.langId = 1;
                        itemController.ViewData["langCode"] = "tr";
                        itemController.ViewData["langId"] = 1;
                        itemController.langCulture = "tr-TR";
                        System.Threading.Thread.CurrentThread.CurrentCulture = trCulture;
                        System.Threading.Thread.CurrentThread.CurrentUICulture = trCulture;

                        break;

                    case "en":
                        itemController.langCode = "en";
                        itemController.langId = 2;
                        itemController.ViewData["langCode"] = "en";
                        itemController.ViewData["langId"] = 2;
                        itemController.langCulture = "en-US";
                        System.Threading.Thread.CurrentThread.CurrentUICulture = enCulture;
                        System.Threading.Thread.CurrentThread.CurrentCulture = enCulture;
                        break;
                } 
            }
            else
            {
                itemController.langCode = "tr";
                itemController.langId = 1;
                itemController.ViewData["langCode"] = "tr";
                itemController.ViewData["langId"] = 1;
                itemController.langCulture = "tr-TR";
                System.Threading.Thread.CurrentThread.CurrentCulture = trCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = trCulture;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}