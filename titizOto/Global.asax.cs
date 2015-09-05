using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using titizOto.HelperAdmin.CustomBinder;

namespace titizOto
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes); 
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            List<string> fileList = new List<string>();
            string val = "";

            fileList = custom.Split(';').ToList();

            foreach (var item in fileList)
            {
                string path = context.Server.MapPath("~/Download/cache/" + item + ".txt");
                if (System.IO.File.Exists(path))
                {
                    var fileLastDate = System.IO.File.GetLastWriteTime(path);

                    val = val + fileLastDate.ToString("dd/MM/yyyy HH:mm:ss");
                }

            }


            return val;

        }
    }
}