using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.App_GlobalResources;
using HelperSite.Shared;
using titizOto.Models;
using ViewModel.ProductList;
using ViewModel.Shared;
using HelperSite.DbController;
using HelperSite.Attribute;

namespace titizOto.Controllers
{
    public class ProductListController : DbWithControllerWithMaster
    {
        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult Index(int pageId)
        {
            var helperPage = new helperProductList();

            // mainProductUrl
            pageShared ps = new pageShared(db);
            var item = ps.getPageById(pageId);

            helperPage.breadCrumbItem = getIndexBreadCrumbProductList(item.name);
            helperPage.header = item.name;

            var newProductList = db.tbl_product.Include("tbl_stock").Include("tbl_gallery").Include("tbl_carModelProduct.tbl_carModel.tbl_carBrand").Where(a => a.statu && a.tbl_stock.Any(b => b.stockCount > 0) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel.tbl_carBrand != null) && a.langId == langId).OrderByDescending(a => a.sequence).ToList();

            productShared pc = new productShared(db);

            int productCount = newProductList.Count;
            int currentPage = pc.getCurrentPage(Request, productCount, 6);
            helperPage.pagingItem = pc.getPageItem(productCount, currentPage, 6, "?sayfa=");


            helperPage.productList = pc.getProductSummary(item.url, newProductList.Skip((currentPage - 1) * 6).Take(6).ToList(), langCulture, langCode, "240", "160");

            // Title
            ps.pageTitleBind(item, helperPage, langId);

            return View(helperPage);
        }

        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult BrandSelect(int carBrandId, string parentUrl, string parentName)
        {
            var helperPage = new helperBrandSelectProductList();

            var carBrand = db.tbl_carBrand.Include("tbl_carModel").Where(a => a.carBrandId == carBrandId).FirstOrDefault();

            helperPage.breadCrumbItem = getBrandSelectBreadCrumbProductList(parentName, parentUrl, carBrand.name);
            helperPage.header = carBrand.name;

            if (!string.IsNullOrEmpty(carBrand.title))
            {
                helperPage.header = carBrand.title;
            }

            helperPage.brandWithModelList = getBrandWithModelList(carBrand, parentUrl, null);

            //productList
            var productListWithBrandId = db.tbl_product.Include("tbl_stock").Include("tbl_gallery").Include("tbl_carModelProduct.tbl_carModel.tbl_carBrand").Where(a => a.statu && a.tbl_stock.Any(b => b.stockCount > 0) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel.tbl_carBrand != null) && a.tbl_carModelProduct.All(c => c.tbl_carModel.tbl_carBrand.carBrandId == carBrandId) && a.langId == langId).OrderBy(a => a.sequence).ToList();

            productShared pc = new productShared(db);
            int productCount = productListWithBrandId.Count;
            int currentPage = pc.getCurrentPage(Request, productCount, 6);
            helperPage.pagingItem = pc.getPageItem(productCount, currentPage, 6, "?sayfa=");

            helperPage.productList = pc.getProductSummary(parentUrl, productListWithBrandId.Skip((currentPage - 1) * 6).Take(6).ToList(), langCulture, langCode, "240", "160");

            // Title
            var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();
            if (settingItem != null)
            {
                helperPage.setBrowserTitle(helperPage.header + settingItem.allPageTitle);
            }

            return View(helperPage);
        }

        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult ModelSelect(int carModelId, int carBrandId, string parentUrl, string parentName)
        {
            var helperPage = new helperModelSelectProductList();

            var carModelItem = db.tbl_carModel.Include("tbl_carBrand").Where(a => a.carModelId == carModelId).FirstOrDefault();
            var carBrandItem = db.tbl_carBrand.Include("tbl_carModel").Where(a => a.carBrandId == carBrandId).FirstOrDefault();
            db.Entry(carBrandItem).Collection(a => a.tbl_carModel).Load();


            helperPage.brandWithModelList = getBrandWithModelList(carBrandItem, parentUrl, carModelItem.url);
            helperPage.breadCrumbItem = getModelSelectBreadCrumbProductList(parentName, parentUrl, carBrandItem.name, carBrandItem.url, carModelItem.name);

            helperPage.header = carBrandItem.name + " " + carModelItem.name;

            if (!string.IsNullOrWhiteSpace(carModelItem.title))
            {
                helperPage.header = carBrandItem.name + " " + carModelItem.title;
            }




            //productList
            var productListWithModelId = db.tbl_product.Include("tbl_stock").Include("tbl_gallery").Include("tbl_carModelProduct.tbl_carModel.tbl_carBrand").Where(a => a.statu && a.tbl_stock.Any(b => b.stockCount > 0) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel.tbl_carBrand != null) && a.tbl_carModelProduct.All(c => c.tbl_carModel.carModelId == carModelId) && a.langId == langId).OrderBy(a => a.sequence).ToList();

            productShared pc = new productShared(db);
            int productCount = productListWithModelId.Count;
            int currentPage = pc.getCurrentPage(Request, productCount, 6);
            helperPage.pagingItem = pc.getPageItem(productCount, currentPage, 6, "?sayfa=");
            helperPage.productList = pc.getProductSummary(parentUrl, productListWithModelId.Skip((currentPage - 1) * 6).Take(6).ToList(), langCulture, langCode, "240", "160");


            // Title
            var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();
            if (settingItem != null)
            {
                helperPage.setBrowserTitle(helperPage.header + settingItem.allPageTitle);
            }

            return View(helperPage);
        }

        [cartSummaryBind]
        public ActionResult AddProduct(int productId)
        {
            System.Threading.Thread.Sleep(2000);

            topCart cartItem = (topCart)ViewData["topCart"];

            basketShared bc = new basketShared(db);
            string returnMsg = "";
            string resultMsg = "";
            string cartHtml = "";
            string hostName = "";
            try
            {
                var result = bc.addProductBasket(productId, cartItem.userId, cartItem.guestGuid, null);

                switch (result)
                {
                    case productAddResult.added:
                        returnMsg = getSuccesMessage(lang.addBasketSuccess, null);
                        resultMsg = "success";
                        cartItem.productCount = cartItem.productCount + 1;
                        cartHtml = RenderRazorViewToString("topCart", cartItem);
                        break;
                    case productAddResult.hasOption:
                        returnMsg = null;
                        resultMsg = "option";
                        hostName = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                        break;
                    case productAddResult.stockError:
                        returnMsg = getErrorMessage(lang.addBasketStockError, null);
                        resultMsg = "error";

                        break;
                    case productAddResult.statuError:
                        returnMsg = getErrorMessage(lang.addBasketStatu, null);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                returnMsg = getErrorMessage(lang.unexpectedErrorMsg, null);
                resultMsg = "error";
                errorSend(ex, "Add Product List");


            }

            return Json(new { result = resultMsg, msgHtml = returnMsg, cartHtml = cartHtml, hostName = hostName });
        }

        [OutputCache(VaryByCustom = "tbl_carModel", Duration = 300)]
        public ActionResult ModelList()
        {
            pageShared ps = new pageShared(db);
            var productUrlWithCategory = ps.getPageByType(pageType.productList, langId);

            var item = getModelList(productUrlWithCategory.url);
            return View(item);
        }

        private brandNameWithModelList getBrandWithModelList(tbl_carBrand carBrand, string productUrl, string modelUrl)
        {
            brandNameWithModelList helper = new brandNameWithModelList();

            helper.brandName = carBrand.name;
            helper.brandUrl = mainPath + langCode + "/" + productUrl + "/" + carBrand.url + ".html";


            helper.modelList = new List<modelList>();

            foreach (var item in carBrand.tbl_carModel.OrderBy(a => a.sequence))
            {
                modelList helperItem = new modelList();
                helperItem.modelName = item.name;
                helperItem.modelUrl = helper.brandUrl.Replace(".html", string.Empty) + "/" + item.url + ".html";

                if (modelUrl == item.url)
                {
                    helperItem.className = "active";
                }

                helper.modelList.Add(helperItem);
            }

            return helper;
        }

        private List<brandList> getModelList(string productUrl)
        {
            List<brandList> helperList = new List<brandList>();

            var list = db.tbl_carBrand.Include("tbl_carModel").Where(a => a.langId == langId && a.statu).OrderBy(a => a.name).ToList();

            foreach (var item in list)
            {
                brandList helperItem = new brandList();

                helperItem.brandName = item.name;
                helperItem.url = mainPath + langCode + "/" + productUrl + "/" + item.url + ".html";

                helperList.Add(helperItem);
            }

            return helperList;
        }

        private breadCrumb getIndexBreadCrumbProductList(string productCategoryName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = productCategoryName;
            helperItem.url = "#";

            return helperItem;
        }

        private breadCrumb getBrandSelectBreadCrumbProductList(string productCategoryName, string productCategoryUrl, string brandName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = productCategoryName;
            helperItem.url = mainPath + langCode + "/" + productCategoryUrl + ".html";

            helperItem.child = new breadCrumb();
            helperItem.child.name = brandName;
            helperItem.child.url = "#";

            return helperItem;
        }

        private breadCrumb getModelSelectBreadCrumbProductList(string productCategoryName, string productCategoryUrl, string brandName, string brandUrl, string modelName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = productCategoryName;
            helperItem.url = mainPath + langCode + "/" + productCategoryUrl + ".html";

            helperItem.child = new breadCrumb();
            helperItem.child.name = brandName;
            helperItem.child.url = mainPath + langCode + "/" + productCategoryUrl + "/" + brandUrl + ".html";

            helperItem.child.child = new breadCrumb();
            helperItem.child.child.name = modelName;
            helperItem.child.child.url = "#";

            return helperItem;
        }
    }
}
