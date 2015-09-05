using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using HelperSite.DbController;
using titizOto.App_GlobalResources;
using titizOto.Models;

namespace titizOto.Controllers
{
    public class NewsletterController : DbWithController
    {
        [HttpPost]
        public ActionResult Add(ViewModel.Newsletter.newsletterItem item)
        {
             
            if (db.tbl_newsletterUser.Any(a => a.email == item.email))
            {
                return Json(new { isSuccess = "No", msg = lang.mailExist });
            }
             
            try
            {
                if (ModelState.IsValid)
                {
                    var newItem = new tbl_newsletterUser();
                    newItem.email = item.email;
                    newItem.createTime = DateTime.Now;
                    newItem.ipNo = getUserIP();

                    db.tbl_newsletterUser.Add(newItem);
                    db.SaveChanges();

                    return Json(new { isSuccess = "Ok", msg = lang.newsletterSuccessMsg });

                }
                else
                {
                    string errorMessages = string.Join("<br />", ModelState.Values.Where(E => E.Errors.Count > 0).SelectMany(E => E.Errors).Select(E => E.ErrorMessage).ToArray());
                    return Json(new { isSuccess = "No", msg = errorMessages });
                }
            }
            catch (Exception ex)
            {
                errorSend(ex, "Newsletter Kayıt İşleminde");
                return Json(new { isSuccess = "No", msg = lang.unexpectedErrorMsg });
            }

        }

        private string getUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

    }
}
