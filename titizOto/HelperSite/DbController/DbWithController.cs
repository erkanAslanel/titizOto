using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using titizOto.Models;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using HelperSite.Interface;

namespace HelperSite.DbController
{
    public class DbWithController : System.Web.Mvc.Controller, IBasicFunction
    {

        public titizOtoEntities db;
        private DbWithBasicFunction dbWithBasicFunction { get; set; }

        public DbWithController()
        {
            dbWithBasicFunction = new DbWithBasicFunction();
            db = dbWithBasicFunction.db;

        }

        public int langId { get; set; }
        public string langCode { get; set; }
        public string langCulture { get; set; }
        public string mainPath { get { return System.Web.VirtualPathUtility.ToAbsolute("~/"); } }

        public void errorSend(Exception ex, string msg)
        {
            errorSend(ex, msg, false);
        }

        public void errorSend(Exception ex, string msg , bool isMailSent)
        {
            dbWithBasicFunction.errorSend(ex, msg);

            if (isMailSent)
            {
                mailSend("erkan.aslanel@gmail.com", "Önemli Hata", "Önemli Hata"); 
            }
           
        }

        public void mailSend(string to, string subject, string body)
        {
            int port = 587;

            if (langId==0)
            {
                langId = 1;
            }

            var siteItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();

            int.TryParse(siteItem.mailPort, out port);
            

            SmtpClient istemci = new SmtpClient(siteItem.mailSmtpServer, port);

            if (siteItem.mailIsEnableSSL.HasValue && siteItem.mailIsEnableSSL.Value)
            {
                istemci.EnableSsl = true;
            }

            istemci.UseDefaultCredentials = true;
            istemci.Credentials = new NetworkCredential(siteItem.mailUserName, siteItem.mailPassword);

            MailAddress sender = new MailAddress(siteItem.mailSentAddress, siteItem.mailSentName);

            MailAddress sendTo = new MailAddress(to);
            MailMessage mail = new MailMessage(sender, sendTo);

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            try
            {
                istemci.Send(mail); 
            }
            catch (Exception ex)
            {
                dbWithBasicFunction.errorSend(ex, "Mail Send Error"); 
            }
            
        }

        public void  mailSend(string subject, string body)
        {
            var siteItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();

            List<string> mailList = siteItem.mailReceiverAddress.Trim().Split(',').ToList();

            foreach (var item in mailList)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    this.mailSend(item, subject, body);
                }
            }
        }

        public void mailSend(string[] toList, string subject, string body)
        {
            foreach (var item in toList)
            {
                mailSend(item, subject, body);
            }

        }
       

        public string getSuccesMessage(string text, string className)
        {
            return dbWithBasicFunction.getSuccesMessage(text, className);
        }

        public string getSuccesMessage(string text)
        {
            return getSuccesMessage(text, "");
        }

        public string getErrorMessage(string text, string className)
        {
            return dbWithBasicFunction.getErrorMessage(text, className);
        }

        public string getErrorMessage(string text)
        {

            return dbWithBasicFunction.getErrorMessage(text, "");
        }

        public string MD5(string text)
        {
            text = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(text, "md5");
            return text;
        }
    }
}