using titizOto.Models;
using HelperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class SettingsController : DbWithController<tbl_settings>
    {
        public ActionResult TestMail(int id)
        {
            var item = getById(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult TestMail(int id, string email)
        {
            var siteItem = getById(id);

            try
            {
                int port = 587;

                int.TryParse(siteItem.mailPort, out port);

                SmtpClient istemci = new SmtpClient(siteItem.mailSmtpServer, port);

                istemci.UseDefaultCredentials = false;
                istemci.Credentials = new NetworkCredential(siteItem.mailUserName, siteItem.mailPassword);
                istemci.Timeout = 2000;

                MailAddress sender = new MailAddress(siteItem.mailSentAddress, siteItem.mailSentName);

                MailAddress sendTo = new MailAddress(email);
                MailMessage mail = new MailMessage(sender, sendTo);

                mail.Subject = "Test Mail " + DateTime.Now.ToString("dd.MM.yyyy");
                mail.Body = "Test Mail";
                mail.IsBodyHtml = true;
                istemci.Send(mail); // Maili gönderdik. 


                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();

            }
            catch (Exception ex)
            {
                errorSend(ex, "Test Maili Gönderme");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            return View(siteItem);
        }
    }
}
