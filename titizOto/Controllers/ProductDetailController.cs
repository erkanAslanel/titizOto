using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.App_GlobalResources;
using HelperSite.Shared;
using titizOto.Models;
using ViewModel.ProductDetail;
using ViewModel.Shared;
using HelperSite.DbController;
using HelperSite.Attribute;


namespace titizOto.Controllers
{
    public class ProductDetailController : DbWithControllerWithMaster
    {
        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult Detail(int productId, int carModelId, int carBrandId, string parentUrl, string parentName)
        {
            helperProductDetail helperPage = new helperProductDetail();

            var productItem = db.tbl_product.Include("tbl_stock").Include("tbl_gallery").Include("tbl_productCritear.tbl_critear").Where(a => a.productId == productId).FirstOrDefault();
            var carBrandItem = db.tbl_carBrand.Where(a => a.carBrandId == carBrandId).FirstOrDefault();
            var carModelItem = db.tbl_carModel.Include("tbl_carModelProduct.tbl_product.tbl_stock").Where(a => a.carModelId == carModelId).FirstOrDefault();

            var baseUrl = mainPath + langCode + "/" + parentUrl + "/" + carBrandItem.url + "/" + carModelItem.url;

            // statu Control
            if (productItem.statu == false)
            {
                return RedirectPermanent(baseUrl + ".html");
            }

            // stockControl
            if (!productItem.tbl_stock.Any(a => a.stockCount > 0))
            {
                return RedirectPermanent(baseUrl + ".html");
            }

            //PrevAndNextUrl 
            var productList = carModelItem.tbl_carModelProduct.Select(a => a.tbl_product).Where(a => a.statu && a.tbl_stock.Any(b => b.stockCount > 0)).OrderBy(a => a.sequence).ToList();

            var sequenceCurrenProduct = productList.IndexOf(productItem);
            helperPage = getPrevAndNextUrl(helperPage, sequenceCurrenProduct, productList, baseUrl);


            //backLink
            helperPage.backLink = baseUrl + ".html";
            if (Request.QueryString["page"] != null)
            {
                // ToDo: Add Pageing Back Link
            }

            productShared pc = new productShared(db);
            CultureInfo priceFormat= CultureInfo.CreateSpecificCulture(langCulture);

            //breadcrumb
            helperPage.breadCrumbItem = getProductDetailBreadCrumbProductList(parentName, parentUrl, carBrandItem.name, carBrandItem.url, carModelItem.name, carModelItem.url, productItem.name);

            helperPage.imageList = pc.getProductGallery(productItem, "500", "350");
            helperPage.productName = productItem.name;
            helperPage.productId = productItem.productId;


            decimal price = pc.calcPriceProduct(productItem);
            helperPage.price = price.ToString("F2",priceFormat) + " TL";

            var taxpriceItem = pc.getProductWithoutTaxPriceAndTaxPrice(productItem, priceFormat);
            helperPage.withoutTaxPrice = taxpriceItem.Item2;

            helperPage.detail = productItem.detail;

            //OptionList
            helperPage.optionList = pc.getOptionListByProductItem(productItem);

            if (Request.QueryString["action"] != null && Request.QueryString["action"] == "optionSelect")
            {
                helperPage.isOptionMsgExist = true;
            }

            //Title
            var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();
            if (settingItem != null)
            {
                helperPage.setBrowserTitle(helperPage.productName + settingItem.allPageTitle);
                helperPage.setDescription(productItem.metaDescription);
                helperPage.setKeywords(productItem.metaKeyword);
            }

            if (settingItem.isCrediCardEnable.HasValue && settingItem.isCrediCardEnable.Value)
            {
                helperPage.isInstallmenTableVisible = true;
            }

            return View(helperPage);
        }

        public ActionResult OptionList(int productId)
        {
            productShared pc = new productShared(db);

            var item = db.tbl_product.Where(a => a.productId == productId).FirstOrDefault();

            if (item == null)
            {
                return null;
            }

            var amount = pc.calcPriceProduct(item);
            var helperPage = getOptionList(amount);
            string viewHtml = RenderRazorViewToString("OptionList", helperPage);
            return Json(new { html = viewHtml });

        }

        public ActionResult OptionListByStr(string  amountStr)
        {
            decimal decAmount = 0;

            decimal.TryParse(amountStr, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-US"), out decAmount);

            return OptionListByDecimal(decAmount);

        }

        public ActionResult OptionListByDecimal(decimal amount)
        { 
         
            var helperPage = getOptionList(amount);
            string viewHtml = RenderRazorViewToString("OptionList", helperPage);
            return Json(new { html = viewHtml });

        }

        private breadCrumb getProductDetailBreadCrumbProductList(string productCategoryName, string productCategoryUrl, string brandName, string brandUrl, string modelName, string modelUrl, string productName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = productCategoryName;
            helperItem.url = mainPath + langCode + "/" + productCategoryUrl + ".html";

            helperItem.child = new breadCrumb();
            helperItem.child.name = brandName;
            helperItem.child.url = mainPath + langCode + "/" + productCategoryUrl + "/" + brandUrl + ".html";

            helperItem.child.child = new breadCrumb();
            helperItem.child.child.name = modelName;
            helperItem.child.child.url = mainPath + langCode + "/" + productCategoryUrl + "/" + brandUrl + "/" + modelUrl + ".html";

            helperItem.child.child.child = new breadCrumb();
            helperItem.child.child.child.name = productName;
            helperItem.child.child.child.url = "#";

            return helperItem;
        }

        private helperProductDetail getPrevAndNextUrl(helperProductDetail helperPage, int sequenceCurrenProduct, List<tbl_product> productList, string baseUrl)
        {
            // prev product Url
            if (sequenceCurrenProduct != 0)
            {
                helperPage.isBackProductUrlExist = true;
                helperPage.backProductUrl = baseUrl + "/" + productList[sequenceCurrenProduct - 1].url + ".html";
            }
            else
            {
                helperPage.isBackProductUrlExist = false;
            }



            // next product Url
            if (sequenceCurrenProduct < productList.Count - 1)
            {
                helperPage.isNextProductUrlExist = true;
                helperPage.nextProductUrl = baseUrl + "/" + productList[sequenceCurrenProduct + 1].url + ".html";
            }
            else
            {
                helperPage.isNextProductUrlExist = false;
            }




            return helperPage;


        }

        private List<payOptionContainer> getOptionList(decimal amount)
        {
            var helper = new List<payOptionContainer>();
            checkoutShared cs = new checkoutShared(db);
            var list = db.tbl_bank.Include("tbl_bankPos.tbl_bankPosOption").Where(a => a.statu).SelectMany(a => a.tbl_bankPos).Where(a => a.statu && a.isPosShownOnTable).ToList();

            List<payOptionItem> blockList = new List<payOptionItem>();


            int index = 1;
            // to Do : installment Tablo 

            foreach (var item in list)
            {
                payOptionItem blockItem = new payOptionItem();

                db.Entry(item).Reference(a => a.tbl_bank).Load();
                db.Entry(item).Collection(a => a.tbl_bankPosOption).Load();

                if (index % 4 == 0)
                {
                    blockItem.containerId = (index / 4);
                }
                else
                {
                    blockItem.containerId = (index / 4) + 1;
                }

                blockItem.logoColor = item.tbl_bank.color1;
                blockItem.headerColor = item.tbl_bank.color2;
                blockItem.rowColor = item.tbl_bank.color3;
                blockItem.logoImg = item.tbl_bank.paymentLogo;
                blockItem.itemList = new List<installmentItem>();

                // 9 Taksit Max
                for (int i = 2; i < 10; i++)
                {
                    installmentItem rowItem = new installmentItem();

                    var dataRowItem = item.tbl_bankPosOption.Where(a => a.paymentCount == i).FirstOrDefault();

                    rowItem.payCount = i;

                    if (dataRowItem != null)
                    {
                        var calc = cs.getInstallmentAmount(amount, i, dataRowItem.additionalAmount);

                        rowItem.totalAmount = calc.Item2;
                        rowItem.insallmentAmount = calc.Item1;

                        if (dataRowItem.minBasketAmount > amount)
                        {
                            rowItem.isMinSpentRequired = true;
                            rowItem.spentAmount = dataRowItem.minBasketAmount;
                        }

                        blockItem.itemList.Add(rowItem);

                    }



                }

                blockItem.excluedeList = blockItem.itemList.Where(a => a.isMinSpentRequired).ToList();


                blockList.Add(blockItem);

                index = index + 1;
            }




            int containerCount = 0;



            if (blockList.Count % 4 == 0)
            {
                containerCount = blockList.Count / 4;
            }
            else
            {
                containerCount = blockList.Count / 4 + 1;
            }

            for (int i = 1; i < containerCount + 1; i++)
            {
                var item = new payOptionContainer();
                item.payOptionList = blockList.Where(a => a.containerId == i).ToList();

                helper.Add(item);
            }


            return helper;
        }


        [HttpPost]
        [cartSummaryBind]
        public ActionResult AddProduct(int productId, string optionValueList)
        {
            System.Threading.Thread.Sleep(2000);

            topCart cartItem = (topCart)ViewData["topCart"];

            basketShared bc = new basketShared(db);

            optionValueList = bc.getOptionList(optionValueList);
            string returnMsg = "";
            string resultMsg = "";
            string cartHtml = "";

            var result = bc.addProductBasket(productId, cartItem.userId, cartItem.guestGuid, optionValueList);
            try
            {
                switch (result)
                {
                    case productAddResult.added:
                        returnMsg = getSuccesMessage(lang.addBasketSuccess, "mRight20 displayTable");
                        resultMsg = "success";
                        cartItem.productCount = cartItem.productCount + 1;
                        cartHtml = RenderRazorViewToString("topCart", cartItem);
                        break;

                    case productAddResult.stockError:
                        returnMsg = getErrorMessage(lang.addBasketStockError, "mRight20 displayTable");
                        resultMsg = "error";

                        break;
                    case productAddResult.statuError:
                        returnMsg = getErrorMessage(lang.addBasketStatu, "mRight20 displayTable");
                        resultMsg = "error";
                        break;

                }
            }

            catch (Exception ex)
            {
                returnMsg = getErrorMessage(lang.unexpectedErrorMsg, null);
                resultMsg = "error";
                errorSend(ex, "Add Product Detail");
            }


            return Json(new { result = resultMsg, msgHtml = returnMsg, cartHtml = cartHtml });

        }


    }
}
