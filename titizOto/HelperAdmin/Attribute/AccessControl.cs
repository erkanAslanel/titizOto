using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelperAdmin
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AccessControl : AuthorizeAttribute
    {
        AccessRole[] roleList;

        public AccessControl(params AccessRole[] roles)
        {
            roleList = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["adminId"] != null && httpContext.Session["roleId"] != null)
            {
                int roleId = int.Parse(httpContext.Session["roleId"].ToString());

                AccessRole enumItem = (AccessRole)roleId;

                if (roleList.Contains(enumItem))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            filterContext.HttpContext.Response.Redirect("~/radmin/login");
        }

    }
}