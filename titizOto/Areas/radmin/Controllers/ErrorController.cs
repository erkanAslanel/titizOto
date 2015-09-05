using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;
using titizOto.Models;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class ErrorController : DbWithController<tbl_error>
    { 
        public ActionResult DeleteAll()
        {
            var item = new tbl_error();

            return View(item);
        }

        [HttpPost]
        public ActionResult DeleteAll(int deleteId)
        {
            try
            {
                db.Database.ExecuteSqlCommand("DELETE FROM tbl_error");
                db.SaveChanges();
                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();
                cacheUpdate();
            }
            catch (Exception ex)
            {
                errorSend(ex, "Delete All Error");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            var item = new tbl_error();

            return View(item);

        } 
    }
}
