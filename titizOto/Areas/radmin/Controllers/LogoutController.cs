using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace titizOto.Areas.radmin.Controllers
{
    public class LogoutController : Controller
    {
         
        public ActionResult Index()
        {
            Session.Clear();

            if (Request.Cookies["adminCookie"] != null)
            { 
                Response.Cookies["adminCookie"].Expires = DateTime.Now.AddDays(-1);   
            }

            return RedirectToRoute("adminLogin"); 
        }

    }
}
