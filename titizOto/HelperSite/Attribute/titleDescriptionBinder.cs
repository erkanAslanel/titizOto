using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.Interface;

namespace HelperSite.Attribute
{
    public class titleDescriptionBinder : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

            var viewModel = filterContext.Controller.ViewData.Model;

            if (viewModel != null && viewModel is IPageable)
            {
                var filterModel = ((IPageable)viewModel);

                if (!string.IsNullOrWhiteSpace(filterModel.getBrowserTitle()))
                {
                    filterContext.Controller.ViewBag.title = filterModel.getBrowserTitle();
                }

                if (!string.IsNullOrWhiteSpace(filterModel.getDescription()))
                {
                    filterContext.Controller.ViewBag.description = filterModel.getDescription();
                }

                if (!string.IsNullOrWhiteSpace(filterModel.getMeta()))
                {
                    filterContext.Controller.ViewBag.meta = filterModel.getMeta();
                }

                if (!string.IsNullOrWhiteSpace(filterModel.getKeywords()))
                {
                    filterContext.Controller.ViewBag.keyword = filterModel.getKeywords();
                }


                if (filterModel.getPageId() != 0)
                {
                    filterContext.Controller.ViewData["pageId"] = filterModel.getPageId();
                }
            }
        }
    }
}