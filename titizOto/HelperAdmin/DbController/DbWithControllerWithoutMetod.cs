using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.Models;

namespace HelperAdmin
{
    public class DbWithControllerWithoutMetod : System.Web.Mvc.Controller, IBasicFunction, IAdminVariable
    {
        public titizOtoEntities db;

        private DbWithBasicFunction dbWithBasicFunction { get; set; }

        public DbWithControllerWithoutMetod()
        {
            dbWithBasicFunction = new DbWithBasicFunction();
            db = dbWithBasicFunction.db;
        } 

        #region generalFunction

        public string getNotification(string notification, string type)
        {
            return ((IBasicFunction)dbWithBasicFunction).getNotification(notification, type);
        }

        public string getNotification(string notification, string type, string classText)
        {
            return ((IBasicFunction)dbWithBasicFunction).getNotification(notification, classText);
        }

        public string getNotificationDefaultSuccess()
        {
            return ((IBasicFunction)dbWithBasicFunction).getNotificationDefaultSuccess();
        }

        public string getNotificationDefaultError()
        {
            return ((IBasicFunction)dbWithBasicFunction).getNotificationDefaultError();
        }

        public string MD5(string text)
        {
            return ((IBasicFunction)dbWithBasicFunction).MD5(text);
        }

        public void errorSend(Exception ex, string msg)
        {
            ((IBasicFunction)dbWithBasicFunction).errorSend(ex, msg);
        }

        public string createUrl(string text)
        {
            return ((IBasicFunction)dbWithBasicFunction).createUrl(text);
        }

 

        #endregion

        #region generalFunctionWithMasterPage

        public AdminVariable getAdminVariable(int userId)
        {
            return ((IAdminVariable)dbWithBasicFunction).getAdminVariable(userId);
        }

        #endregion


        public string getSiteName(HttpRequestBase Request)
        {
            return ((IBasicFunction)dbWithBasicFunction).getSiteName(Request);
        }

        public string getSiteNameWithoutSlash(HttpRequestBase Request)
        {
            return ((IBasicFunction)dbWithBasicFunction).getSiteNameWithoutSlash(Request);
        }
    }
}