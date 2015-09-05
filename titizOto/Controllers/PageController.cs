using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.Attribute;
using HelperSite.DbController;
using HelperSite.Shared;
using titizOto.Models;
using ViewModel.Page;
using ViewModel.Shared;


namespace titizOto.Controllers
{
    public class PageController : DbWithControllerWithMaster
    {
        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult Index(int pageId)
        {
            helperStaticPage helperItem = new helperStaticPage();
            pageShared ps = new pageShared(db);

            var item = db.tbl_page.Include("tbl_category").Where(a => a.pageId == pageId).FirstOrDefault();

            if (item.tbl_category == null)
            {
                return null;
            }

            var parentCategoryList = db.tbl_category.Include("tbl_page").Where(a => a.parentId == item.tbl_category.parentId).ToList();
            var selectedUrl = ps.getPageUrl(item);

            helperItem.leftMenuList = getStaticLeftMenu(parentCategoryList, pageId);
            helperItem.breadCrumbItem = getBreadCrumbStaticPage(item.name);
            helperItem.pageTitle = item.name;

            if (!string.IsNullOrWhiteSpace(item.title))
            {
                helperItem.pageTitle = item.title;
            }

            helperItem.detail = item.detail;

            //Title
            ps.pageTitleBind(item, helperItem, langId);


            return View(helperItem);
        }

        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult OpenContent(int pageId)
        {

            helperOpenContent helperItem = new helperOpenContent();
            pageShared ps = new pageShared(db);

            var item = db.tbl_page.Include("tbl_category").Where(a => a.pageId == pageId).FirstOrDefault();

            helperItem.pageTitle = item.name;

            if (!string.IsNullOrWhiteSpace(item.title))
            {
                helperItem.pageTitle = item.title;
            }

            var parentCategoryList = db.tbl_category.Include("tbl_page").Where(a => a.parentId == item.tbl_category.parentId && a.statu).ToList();
            var selectedUrl = ps.getPageUrl(item);

            helperItem.leftMenuList = getStaticLeftMenu(parentCategoryList, pageId);

            helperItem.breadCrumbItem = getBreadCrumbStaticPage(item.name);

            helperItem.list = db.tbl_openContent.Where(a => a.langId == langId && a.categoryId == item.categoryId && a.statu).OrderBy(a => a.sequence).Select(a => new openContentItem() { title = a.title, detail = a.detail }).ToList();

            //Title
            ps.pageTitleBind(item, helperItem, langId);

            return View(helperItem);

        }

        private breadCrumb getBreadCrumbStaticPage(string pageName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = pageName;
            helperItem.url = "#";

            return helperItem;
        }

        private List<leftMenuItem> getStaticLeftMenu(List<tbl_category> list, int selectedPageId)
        {
            List<leftMenuItem> helperList = new List<leftMenuItem>();

            pageShared ps = new pageShared(db);

            foreach (var item in list.Where(a=>a.statu).OrderBy(a => a.sequence))
            {
                leftMenuItem helper = new leftMenuItem();

                helper.name = item.name;
                var pageItem = item.tbl_page.Where(a => a != null && a.statu).FirstOrDefault();

                if (pageItem != null)
                {
                    helper.url = mainPath + langCode + "/" + ps.getPageUrl(pageItem) + ".html";

                    if (pageItem.pageId == selectedPageId)
                    {
                        helper.className = "active";
                    }
                }
                else
                {
                    helper.url = "#";
                }



                helperList.Add(helper);

            }


            return helperList;
        }


    }
}
