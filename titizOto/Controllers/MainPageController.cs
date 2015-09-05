using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.Attribute;
using HelperSite.DbController;
using HelperSite.Shared;
using titizOto.Models;
using ViewModel.MainPage;
using ViewModel.Shared;

namespace titizOto.Controllers
{
    public class MainPageController : DbWithControllerWithMaster
    {
        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult Index()
        {
            pageShared ps = new pageShared(db);

            helperMainPage helperPage = new helperMainPage();

            // Page All Brand
            var pageAllBrand = ps.getPageByType(pageType.allBrand, langId); 
            if (pageAllBrand != null)
            {
                helperPage.brandUrl = mainPath + langCode + "/" + pageAllBrand.url + ".html";
                helperPage.allBrandName = pageAllBrand.name;
            }


            // Page Product List
            var pageProductList = ps.getPageByType(pageType.productList, langId); 
            if (pageProductList != null)
            {
                helperPage.newProductList = getNewProductList(pageProductList.url);
            }

            var settingItem = db.tbl_settings.Where(a => a.langId == 1).FirstOrDefault();
            if (settingItem != null)
            {
                helperPage.title = settingItem.mainPageTitle;
                helperPage.browserTitle = settingItem.mainPageTitle;
                helperPage.description = settingItem.metaDescription;
                helperPage.meta = settingItem.metaDescriptionAdditional;
                helperPage.keyword = settingItem.metaKeyword;

            }

            return View(helperPage);
        }
          
        // Modeller
        private List<carBrand> getCarBrandList(string productUrl)
        {
            List<carBrand> helperList = new List<carBrand>();

            var list = db.tbl_carBrand.Where(a => a.statu && a.isMainPageShown && a.langId == langId).OrderBy(a => a.sequence).Take(18).ToList();

            int itemIndex = 1;

            foreach (var item in list)
            {
                carBrand helper = new carBrand();

                if (itemIndex % 3 == 0)
                {
                    helper.classText = "mRight0";
                }

                helper.name = item.name;
                helper.url = mainPath + langCode + "/" + productUrl + "/" + item.url + ".html";
                helper.photo = mainPath + "ImageShow/carBrand/" + item.photo + "/" + item.photoCoordinate + "/60/60/1";

                helperList.Add(helper);
                itemIndex++;
            }



            return helperList;

        }

        // New Product List
        private List<productSmall> getNewProductList(string productUrl)
        {
            var list = db.tbl_product.Include("tbl_stock").Include("tbl_gallery").Include("tbl_carModelProduct.tbl_carModel.tbl_carBrand").Where(a => a.isShowCase && a.statu && a.tbl_stock.Any(b => b.stockCount > 0) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel.tbl_carBrand != null) && a.langId == langId).Take(6).ToList();

            productShared pc = new productShared(db);

            return pc.getProductSummary(productUrl, list, langCulture, langCode);
        }

        // Car Brand 
        [OutputCache(VaryByCustom = "tbl_carBrand", Duration = 300)]
        public ActionResult brandList()
        {
            pageShared ps = new pageShared(db);

            var productPage = ps.getPageByType(pageType.productList,langId);

            if (productPage != null)
            {
                var item = getCarBrandList(productPage.url);
                return PartialView(item);
            }
            else
            {
                return null;
            }

        }

        // Slider 
        [OutputCache(VaryByCustom = "tbl_slider", Duration = 300)]
        public ActionResult slider()
        {
            List<slider> itemList = new List<slider>();

            var list = db.tbl_slider.Where(a => a.statu && a.langId == langId).OrderBy(a => a.sequence).ToList();

            foreach (var item in list)
            {
                slider helperItem = new slider();
                helperItem.isHaveUrl = item.isUrlActive;
                helperItem.photo = mainPath + "Download/item/slider/" + item.photo;
                helperItem.title = item.title;
                helperItem.subTitle = item.subTitle;
                helperItem.url = item.urlText;

                itemList.Add(helperItem);
            } 

            return PartialView(itemList);
        } 

    }
}
