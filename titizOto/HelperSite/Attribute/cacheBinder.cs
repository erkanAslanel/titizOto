using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelperSite.Attribute
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class cacheBinder : ActionFilterAttribute
    {
        string[] tableList;

        public cacheBinder(params string[] fileListParam)
        {
            this.tableList = fileListParam;
        }

       

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext); 

            List<string> fileList = new List<string>();


            foreach (var item in tableList)
            {
                string path = filterContext.RequestContext.HttpContext.Server.MapPath("~/Download/cache/" + item + ".txt");
                if (System.IO.File.Exists(path))
                {
                    fileList.Add(path);
                }

            }

            if (tableList.Count() > 0)
            {

                filterContext.HttpContext.Response.AddFileDependencies(fileList.ToArray());
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddMinutes(2));
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Server);
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(true);
               
            }
        }
    }
}