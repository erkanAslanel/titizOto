using titizOto.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.Attribute;
using Newtonsoft.Json;



namespace HelperSite.DbController
{
    [langBind]
    public class DbWithControllerWithMaster : DbWithController
    {
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }


        public string RenderRazorViewToString(string viewName, object model, ControllerContext ControllerContext)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public string serializeObject(object item)
        {
            try
            {
                return JsonConvert.SerializeObject(item, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
            );
            }
            catch (Exception ex)
            {
                return ex.Message + "Json Serilaze Problem";

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

        public string getModelStateError(ModelStateDictionary ModelState)
        {
            string messages = string.Join("<br/> ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));


            return messages;
        }

    }
}