using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelperAdmin
{
    interface IBasicFunction
    { 
          string createUrl(string text);

        /// <summary>
        /// Warning, Information, Success, Failure
        /// </summary>
         string getNotification(string notification, string type);

        /// <summary>
        /// Warning, Information, Success, Failure
        /// </summary>
         string getNotification(string notification, string type, string classText);

         string getNotificationDefaultSuccess();

         string getNotificationDefaultError();

         string MD5(string text);

         void errorSend(Exception ex, string msg);

         string getSiteName(System.Web.HttpRequestBase Request);

         string getSiteNameWithoutSlash(System.Web.HttpRequestBase Request);

    }
}
