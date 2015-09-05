using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace HelperAdmin
{
    public class DbWithBasicFunction : DbConnector, IBasicFunction, IAdminVariable
    {


        #region GeneralFunction

        public string createUrl(string text)
        {
            text = text.Replace("ı", "i");
            text = text.Replace("İ", "i");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ç", "c");
            text = text.Replace("Ç", "C");
            text = text.Replace("ş", "s");
            text = text.Replace("Ş", "S");
            text = text.Replace("ö", "o");
            text = text.Replace("Ö", "O");
            text = text.Replace("ü", "u");
            text = text.Replace("Ü", "U");
            text = text.Replace("I", "i");
            text = text.Replace(" ", "-");
            text = text.Replace("[", "-");
            text = text.Replace("]", "-");
            text = text.Replace("(", "-");
            text = text.Replace(")", "-");
            text = text.Replace("|", "-");
            text = text.Replace(":", "-");
            text = text.Replace(";", "-");
            text = text.Replace(".", "-");
            text = text.Replace(",", "-");
            text = text.Replace("~", "-");
            text = text.Replace("!", "");
            text = text.Replace("\\", "");
            text = text.Replace("/", "");
            text = text.Replace("&", "");
            text = text.Replace("+", "-");
            text = text.Replace("'", "");
            text = text.Replace("?", "");
            text = text.Replace("#", "");
            text = text.Replace("\"", "");
            text = text.Trim();
            text = text.ToLower();
            return text;
        }

        /// <summary>
        /// Warning, Information, Success, Failure
        /// </summary>
        public string getNotification(string notification, string type)
        {
            return "<div class=\"nNote n" + type + " hideit\"><p>" + notification + "</p></div>";
        }

        /// <summary>
        /// Warning, Information, Success, Failure
        /// </summary>
        public string getNotification(string notification, string type, string classText)
        {
            return "<div class=\"" + classText + " nNote n" + type + " hideit\"><p>" + notification + "</p></div>";
        }

        public string getNotificationDefaultSuccess()
        {
            return getNotification("İşleminiz başarıyla gerçekleşti.", "Success", "mb10 disappear");
        }

        public string getNotificationDefaultError()
        {
            return getNotification("Beklenmedik hata meydana geldi.", "Failure", "mb10");
        }

        public string MD5(string text)
        {
            text = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(text, "md5");
            return text;
        }

        public void errorSend(Exception ex, string msg)
        {
            string path = HttpContext.Current.Server.MapPath("~/Download/error/error.txt");
            string errorText = DateTime.Now.ToString("dd.MM.yyyy") + "--" + msg + "--" + ex.Message;
            if (!System.IO.File.Exists(path))
            {
                var fileCreater = System.IO.File.Create(path);
                fileCreater.Dispose();
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(errorText);
            }

            try
            {
                tbl_error item = new tbl_error();

                item.saveDate = DateTime.Now;
                item.errorText = errorText;
                db.tbl_error.Add(item);
                db.SaveChanges();
            }
            catch
            {

            }
        }

        public string getSiteName(HttpRequestBase Request)
        {

            return Request.Url.Scheme + "://" + Request.Url.Authority +
        Request.ApplicationPath.TrimEnd('/') + "/";

        }

        public string getSiteNameWithoutSlash(HttpRequestBase Request)
        {

            return Request.Url.Scheme + "://" + Request.Url.Authority +
        Request.ApplicationPath.TrimEnd('/');

        }

        #endregion

        public AdminVariable getAdminVariable(int userId)
        {
            AdminVariable item = new AdminVariable();

            tbl_adminUser user_Item = db.tbl_adminUser.Where(a => a.userId == userId).FirstOrDefault();
            //int count = db.tbl_contact.Where(a => a.status).Count();

            if (user_Item != null)
            {
                item.name = user_Item.name;
                item.surname = user_Item.surname;
                //item.messageCount = count;
            }

            var os = new HelperSite.Shared.orderShared(db);
            var waitinOrderList = os.getWaitingOrderStatuList();
            item.waitingOrder = db.tbl_order.Where(a => waitinOrderList.Contains(a.orderStatu)).Count();


            return item;
        }
    }
}