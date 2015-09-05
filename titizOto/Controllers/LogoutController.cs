using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.DbController;

namespace titizOto.Controllers
{
    public class LogoutController : DbWithControllerWithMaster
    {


        public ActionResult Index()
        {
            Session.Remove("userId");
            Session.Remove("checkoutProcess");

            if (Request.Cookies["userCookie"] != null)
            {
                Response.Cookies["userCookie"].Expires = DateTime.Now.AddDays(-1);
            }

            return Redirect("~/" + langCode);
        }

    }
}
