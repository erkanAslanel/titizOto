using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.App_GlobalResources;
using HelperSite.Shared;
using titizOto.Models;
using ViewModel.Basket;
using ViewModel.Shared;
using HelperSite.DbController;
using HelperSite.Attribute;

namespace titizOto.Controllers
{
    public class BasketController : DbWithControllerWithMaster
    {
        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult Index(int pageId)
        {
            topCart cartItem = (topCart)ViewData["topCart"];
            basketShared bs = new basketShared(db);
            pageShared ps = new pageShared(db);
            discountShared dc = new discountShared(db);

            // get Basket Content => Price , Discount 
            helperBasket helperPage = new helperBasket();
            var basketContent = bs.getBasketHelperWithProductAndDiscount(cartItem, langId, langCode, mainPath,false);
            if (basketContent.Item2 == basketActionResult.redirect)
            {
                return returnBasketMainPage(null);
            }

            helperPage = basketContent.Item1;


            var pageItem = ps.getPageById(pageId);
            if (pageItem == null)
            {
                return null;
            }

            helperPage.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);
            ps.pageTitleBind(pageItem, helperPage, langId);
            helperPage.updateBasketUrl = pageItem.url;

            var checkoutPage = ps.getPageByType(pageType.checkoutMain, langId);
            if (checkoutPage != null)
            {
                helperPage.checkoutUrl = langCode + "/" + checkoutPage.url + ".html";
            }


            var productPage = ps.getPageByType(pageType.productList, langId);
            helperPage.continueShopUrl = mainPath + langCode + "/" + productPage.url + ".html";

            if (Session["checkoutProcess"] != null)
            {
                var processItem = (ViewModel.Checkout.checkoutProcess)Session["checkoutProcess"]; 
                processItem.isCartItemChange = false;

                Session["checkoutProcess"] = processItem;
            }

            // Discount Message
            if (Request.QueryString["discount"] != null)
            {
                string discountMessage = getDiscountErrorByString(Request.QueryString["discount"]);

                if (discountMessage != null)
                {
                    helperPage.isMessageExist = true;
                    helperPage.message = discountMessage;
                }
            }

            // Adjust Message
            if (helperPage.actionMsg == basketActionResult.stockAdjust)
            {
                helperPage.isMessageExist = true;
                helperPage.message = getErrorMessage(lang.stockAdjust, "warning autoHide");
                return View(helperPage);
            }

            // Update Message
            if (Request.QueryString["update"] != null)
            {
                string updateType = Request.QueryString["update"];

                if (updateType == "success")
                {
                    helperPage.isMessageExist = true;
                    helperPage.message = getSuccesMessage(lang.basketSuccessOperation, "autoHide");
                }

                if (updateType == "productUpdateSuccess")
                {
                    helperPage.isMessageExist = true;
                    helperPage.message = getSuccesMessage(lang.basketSuccessUpdateProduct, "autoHide");
                }

                if (updateType == "deleteDiscount")
                {
                    helperPage.isMessageExist = true;
                    helperPage.message = getSuccesMessage(lang.basketSuccessDeleteDiscount, "autoHide");
                }

                if (updateType == "deleteProduct")
                {
                    helperPage.isMessageExist = true;
                    helperPage.message = getSuccesMessage(lang.basketSuccessDeleteItem, "autoHide");
                }

                if (updateType == "errorQuantity")
                {
                    helperPage.isMessageExist = true;
                    helperPage.message = getErrorMessage(lang.basketErrorQuantity, "autoHide");
                }

                if (updateType == "error")
                {
                    helperPage.isMessageExist = true;
                    helperPage.message = getErrorMessage(lang.unexpectedErrorMsg, "autoHide");
                }

                if (updateType == "onCheckout")
                {
                    helperPage.isMessageExist = true;
                    helperPage.message = getErrorMessage(lang.onCheckoutBasketChange, "autoHide");
                }
            }

            return View(helperPage);
        }

        [HttpPost]
        [cartSummaryBind]
        public ActionResult Update()
        {
            basketShared bc = new basketShared(db);
            discountShared dc = new discountShared(db);
            int basketId = 0;
            string basketIdStr = "";

            foreach (string item in Request.Form.Keys)
            {
                // Update Product Count
                if (item.IndexOf("quantityUpdate_") != -1)
                {
                    basketIdStr = item.Replace("quantityUpdate_", string.Empty);
                    int updatedQuantity = 0;

                    if (int.TryParse(basketIdStr, out basketId) && Request.Form["quantityBox_" + basketId.ToString()] != null && int.TryParse(Request.Form["quantityBox_" + basketId.ToString()], out updatedQuantity))
                    {
                        var basketItemObj = db.tbl_basket.Where(a => a.basketId == basketId).FirstOrDefault();

                        try
                        {
                            bc.updateStockCount(basketId, updatedQuantity);
                            return returnBasketMainPage("update=productUpdateSuccess");
                        }
                        catch (Exception ex)
                        {
                            errorSend(ex, "Basket Update Product Count", true);
                            return returnBasketMainPage("update=error");
                        }
                    }
                    else
                    {
                        return returnBasketMainPage("update=errorQuantity");
                    }

                    
                }

                // Delete Discount
                if (item.IndexOf("deleteDiscount_") != -1)
                {
                    string discountIdStr = item.Replace("deleteDiscount_", string.Empty);
                    int discountId = 0;
                    if (int.TryParse(discountIdStr, out discountId))
                    {
                        topCart cartItem = (topCart)ViewData["topCart"];
                        var basketContent = bc.getBasketContent(cartItem, langId, langCode, mainPath,false);

                        try
                        {
                            dc.deleteDiscountOnBasketByDiscountId(basketContent.Item1, discountId);
                            return returnBasketMainPage("update=deleteDiscount");

                        }
                        catch (Exception ex)
                        {
                            errorSend(ex, "delete Discount");
                            return returnBasketMainPage("update=error");
                        }

                    }

                    break;
                }

                // Delete Basket 
                if (item.IndexOf("delete_") != -1)
                {
                    basketIdStr = item.Replace("delete_", string.Empty);
                    if (int.TryParse(basketIdStr, out basketId))
                    {
                        try
                        {
                            bc.deleteBasketById(basketId);
                            return returnBasketMainPage("update=deleteProduct");

                        }
                        catch (Exception ex)
                        {
                            errorSend(ex, "basket Delete");
                            return returnBasketMainPage("update=error");
                        }

                    }

                    break;
                }
            }


            return returnBasketMainPage("update=error");


        }

        private breadCrumb getBreadCrumbStaticPage(string pageName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = pageName;
            helperItem.url = "#";

            return helperItem;
        }

        [cartSummaryBind]
        [HttpPost]
        public ActionResult AddDiscount(string discountCode)
        {
            topCart cartItem = (topCart)ViewData["topCart"];
            basketShared bc = new basketShared(db);
            discountShared dc = new discountShared(db);

            var basketContent = bc.getBasketContent(cartItem, langId, langCode, mainPath,false);
            var addObject = dc.isDiscountValidForBasketContent(basketContent.Item1, discountCode, cartItem.userId);

            if (dc.isAllreadyUse(basketContent.Item1, addObject.Item2))
            {
                return returnBasketMainPage("discount=" + discountErrorType.alreadyInUse.ToString());
            }

            if (addObject.Item1)
            {
                try
                {
                    dc.addDiscountBasket(basketContent.Item1, discountCode);
                    return returnBasketMainPage("discount=" + discountErrorType.success.ToString());
                }
                catch (Exception ex)
                {
                    errorSend(ex, "Add Discount Error", true);
                    return returnBasketMainPage("discount=" + discountErrorType.unExpected.ToString());
                }
            }
            else
            {

                return returnBasketMainPage("discount=" + addObject.Item3.ToString());
            }

        }

        public string getDiscountErrorByString(string text)
        {
            var item = discountErrorType.none;

            Enum.TryParse<discountErrorType>(text, out item);

            string returnMsg = null;

            switch (item)
            {
                case discountErrorType.notFoundInSystem:

                    returnMsg = lang.discountErrorNotFoundInSystem;

                    break;
                case discountErrorType.productError:

                    returnMsg = lang.discountErrorProduct;
                    break;
                case discountErrorType.repTime:

                    returnMsg = lang.discountErrorRepTime;
                    break;

                case discountErrorType.minSpent:

                    returnMsg = lang.discountErrorMinSpent;

                    break;
                case discountErrorType.minCount:

                    returnMsg = lang.discountErrorMinCount;
                    break;
                case discountErrorType.date:

                    returnMsg = lang.discountErrorMinCount;

                    break;
                case discountErrorType.user:

                    returnMsg = lang.discountErrorUser;

                    break;
                case discountErrorType.unExpected:
                    returnMsg = lang.unexpectedErrorMsg;

                    break;

                // Success
                case discountErrorType.success:

                    return getSuccesMessage(lang.discountSuccess, "autoHide");

                case discountErrorType.alreadyInUse:


                    returnMsg = lang.discountErrorAlreadyUse;

                    break;

            }


            return getErrorMessage(returnMsg, "autoHide");


        }

        public RedirectResult returnBasketMainPage(string query)
        {
            pageShared ps = new pageShared(db);

            var basketPage = ps.getPageByType(pageType.basket, langId);
            if (basketPage != null)
            {
                if (query != null)
                {
                    return Redirect("~/" + langCode + "/" + basketPage.url + ".html" + "?" + query);
                }
                else
                {
                    return Redirect("~/" + langCode + "/" + basketPage.url + ".html");
                }
            }

            else
            {
                return Redirect("~/");
            }

        }
    }
}
