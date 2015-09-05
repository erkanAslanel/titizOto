using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.DbController;
using HelperSite.Shared;

namespace HelperSite.Attribute
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class userTypeControl : AuthorizeAttribute
    {
        userType[] roleList;

        public userTypeControl(params userType[] roles)
        {
            roleList = roles;
        } 

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isValid = false;

            if (httpContext.Session["userId"] != null && httpContext.Session["userRoleId"] != null)
            {
                int roleId = int.Parse(httpContext.Session["userRoleId"].ToString());
                userType enumItem = (userType)roleId;

                if (roleList.Contains(enumItem))
                {
                    isValid = true;
                }

            }


            return isValid;
        } 
        
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            DbWithController itemController = (DbWithController)filterContext.Controller; ;

            pageShared ps = new pageShared(itemController.db);

            var loginPage = ps.getPageByType(pageType.registerLogin, itemController.langId);

            if (loginPage != null)
            {
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


                filterContext.HttpContext.Session["redirectPage"] = filterContext.HttpContext.Request.Url.ToString();
                filterContext.HttpContext.Response.Redirect("~/" + itemController.langCode + "/" + loginPage.url + ".html?needLogin=yes", true);
            }
            else
            {
                filterContext.HttpContext.Response.Redirect("~/", true);
            }
        }

        
    }
}