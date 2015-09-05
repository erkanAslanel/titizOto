using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.Attribute;
using HelperSite.DbController;
using HelperSite.Shared;
using ViewModel.AllBrand;
using ViewModel.Shared;

namespace titizOto.Controllers
{
    public class AllBrandController : DbWithControllerWithMaster
    {
        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult Index(int pageId)
        {
            helperAllBrand helperItem = new helperAllBrand();

            // Brand List
            pageShared ps = new pageShared(db);
            var productList = ps.getPageByType(pageType.productList ,langId);
            helperItem.brandList = getBrandList(productList.url);

            // All BranName  
            pageShared pc = new pageShared(db);
            var brandPageItem = pc.getPageById(pageId);
            helperItem.brandName = brandPageItem.name;
            helperItem.breadCrumbItem = getBreadCrumbAllBrand(brandPageItem.name);

            // Title
            pc.pageTitleBind(brandPageItem, helperItem, langId);

            return View(helperItem);
        }

        private List<allBrand> getBrandList(string productListUrl)
        {
            List<allBrand> helperList = new List<allBrand>();

            var list = db.tbl_carBrand.Where(a => a.statu).OrderBy(a => a.name).ToList();

            foreach (var item in list)
            {
                allBrand helperItem = new allBrand();

                helperItem.photo = mainPath + "ImageShow/carBrand/" + item.photo + "/" + item.photoCoordinate + "/60/60/1";
                helperItem.name = item.name;
                helperItem.url = mainPath + langCode + "/" + productListUrl + "/" + item.url + ".html";
                helperList.Add(helperItem);

            } 

            return helperList; 
        }

        private breadCrumb getBreadCrumbAllBrand(string brandName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = brandName;
            helperItem.url = "#";

            return helperItem;
        }
    }
}
