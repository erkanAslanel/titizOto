using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.DbController;
using HelperSite.Shared;
using ViewModel.Checkout;
using ViewModel.Shared;

namespace HelperSite.Attribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class sessionCheckoutControl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            DbWithController itemController = (DbWithController)filterContext.Controller;

            if (filterContext.Controller.ViewData["topCart"] != null)
            {
                topCart cartItem = (topCart)filterContext.Controller.ViewData["topCart"];
                checkoutProcess processItem = null;

                // StepLink Allways Update
                var stepLink = getCheckoutStepList(itemController.db, itemController.langId, itemController.langCode);

                // checkout normal adımları, öncesinde checkout objesi oluşmuş
                if (filterContext.RequestContext.HttpContext.Session["checkoutProcess"] != null)
                {
                    processItem = (checkoutProcess)filterContext.RequestContext.HttpContext.Session["checkoutProcess"];


                    if (!isCartSame(processItem.cartItem, cartItem))
                    {
                        processItem.isCartItemChange = true;
                        processItem.cartItem = cartItem;
                    }

                    processItem.stepLinkList = stepLink;

                } // ilk checkouta giren adam
                else
                {
                    // Right Step Link
                    processItem = new checkoutProcess(stepLink, cartItem);

                }

                filterContext.RequestContext.HttpContext.Session["checkoutProcess"] = processItem;
            }
            else
            {
                throw new Exception("CartItem Null geliyor.Chekout processin başlangıcı sessionCheckoutControl");
            }
        }

        public List<checkoutPageItem> getCheckoutStepList(titizOto.Models.titizOtoEntities db, int langId, string langCode)
        {
            List<checkoutPageItem> list = new List<checkoutPageItem>();

            pageShared ps = new pageShared(db);
            checkoutPageItem item = null;

            int[] pageTypeList = new int[] { (int)pageType.checkoutRegisterStatu, (int)pageType.checkoutDelivery, (int)pageType.checkoutBilling, (int)pageType.checkoutCargo, (int)pageType.checkoutPayment, (int)pageType.checkoutSummary, (int)pageType.checkoutMain, (int)pageType.checkoutComplete, (int)pageType.checkoutErrorProcess };


            var pageList = db.tbl_page.Where(a => pageTypeList.Contains(a.pageTypeId) && a.langId == langId).ToList();
            var mainCheckoutPage = pageList.Where(a => a.pageTypeId == (int)pageType.checkoutMain).FirstOrDefault();

            // register Statu 
            var pageItem = pageList.Where(a => a.pageTypeId == (int)pageType.checkoutRegisterStatu).FirstOrDefault();
            if (pageItem != null)
            {
                item = getCheckoutPageByPageItemAndPageType(pageItem, checkoutStep.registerOptions, langCode, mainCheckoutPage.url);
                list.Add(item);
            }

            // deliverty 
            pageItem = pageList.Where(a => a.pageTypeId == (int)pageType.checkoutDelivery).FirstOrDefault();
            if (pageItem != null)
            {
                item = getCheckoutPageByPageItemAndPageType(pageItem, checkoutStep.delivery, langCode, mainCheckoutPage.url);
                list.Add(item);
            }

            // billing 
            pageItem = pageList.Where(a => a.pageTypeId == (int)pageType.checkoutBilling).FirstOrDefault();
            if (pageItem != null)
            {
                item = getCheckoutPageByPageItemAndPageType(pageItem, checkoutStep.billing, langCode, mainCheckoutPage.url);
                list.Add(item);
            }

            // cargo 
            pageItem = pageList.Where(a => a.pageTypeId == (int)pageType.checkoutCargo).FirstOrDefault();
            if (pageItem != null)
            {
                item = getCheckoutPageByPageItemAndPageType(pageItem, checkoutStep.cargo, langCode, mainCheckoutPage.url);
                list.Add(item);
            }

            // paymentOptions 
            pageItem = pageList.Where(a => a.pageTypeId == (int)pageType.checkoutPayment).FirstOrDefault();
            if (pageItem != null)
            {
                item = getCheckoutPageByPageItemAndPageType(pageItem, checkoutStep.payment, langCode, mainCheckoutPage.url);
                list.Add(item);
            }

            // summary 
            pageItem = pageList.Where(a => a.pageTypeId == (int)pageType.checkoutSummary).FirstOrDefault();
            if (pageItem != null)
            {
                item = getCheckoutPageByPageItemAndPageType(pageItem, checkoutStep.summary, langCode, mainCheckoutPage.url);
                list.Add(item);
            }

            // Important Error 
            pageItem = pageList.Where(a => a.pageTypeId == (int)pageType.checkoutErrorProcess).FirstOrDefault();
            if (pageItem != null)
            {
                item = getCheckoutPageByPageItemAndPageType(pageItem, checkoutStep.error, langCode, mainCheckoutPage.url);
                list.Add(item);
            }

            // Complete
            pageItem = pageList.Where(a => a.pageTypeId == (int)pageType.checkoutComplete).FirstOrDefault();
            if (pageItem != null)
            {
                item = getCheckoutPageByPageItemAndPageType(pageItem, checkoutStep.complete, langCode, mainCheckoutPage.url);
                list.Add(item);
            }


            return list;

        }

        public checkoutPageItem getCheckoutPageByPageItemAndPageType(titizOto.Models.tbl_page pageItem, checkoutStep pageType, string langCode, string mainCheckoutPageUrl)
        {
            var item = new checkoutPageItem();
            item.name = pageItem.name;
            item.url = langCode + "/" + mainCheckoutPageUrl + "/" + pageItem.url + ".html";
            item.step = pageType;

            return item;
        }

        public bool isCartSame(topCart obj1, topCart obj2)
        {
            bool isSame = true;

            if (obj1.basketUrl != obj2.basketUrl)
            {
                isSame = false;
            }

            if (obj1.guestGuid != obj2.guestGuid)
            {
                isSame = false;
            }

            if (obj1.isRegisteredUser != obj2.isRegisteredUser)
            {
                isSame = false;
            }

            if (obj1.nameSurname != obj2.nameSurname)
            {
                isSame = false;
            }

            if (obj1.productCount != obj2.productCount)
            {
                isSame = false;
            }

            if (obj1.registerUrl != obj2.registerUrl)
            {
                isSame = false;
            }

            if (obj1.userGuid != obj2.userGuid)
            {
                isSame = false;
            }

            if (obj1.userId != obj2.userId)
            {
                isSame = false;
            }

            if (obj1.basketIdString != obj2.basketIdString)
            {
                isSame = false;
            }

            return isSame;
        }
    }
}