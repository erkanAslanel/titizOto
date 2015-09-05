using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelperSite.Interface;
using titizOto.Models;

namespace HelperSite.DbController
{
    public class DbWithBasicFunction : DbConnector, IBasicFunction
    { 

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

                var dbContext = new titizOtoEntities();
                dbContext.tbl_error.Add(item);
                dbContext.SaveChanges();
                dbContext.Dispose();

            }
            catch
            {

            } 
          
        }
         

        public string getSuccesMessage(string text,string className)
        {
            string msg = "<div class=\"msg success [className]\"> [msgText] <span class=\"msgclose\">x<span> </div>";
            msg = msg.Replace("[msgText]", text);
            msg = msg.Replace("[className]", className);
            return msg;
        }

        public string getErrorMessage(string text, string className)
        {
            string msg = "<div class=\"msg error [className]\"> [msgText] <span class=\"msgclose\">x<span>  </div>";
            msg = msg.Replace("[msgText]", text);
            msg = msg.Replace("[className]", className);
            return msg;
        }
    }
}