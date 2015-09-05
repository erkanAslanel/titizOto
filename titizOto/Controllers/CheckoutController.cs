using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.Attribute;
using HelperSite.DbController;
using HelperSite.Shared;
using ViewModel.Checkout;
using ViewModel.Checkout.Billing;
using ViewModel.Checkout.CheckoutOption;
using ViewModel.Checkout.Delivery;
using ViewModel.Checkout.Cargo;
using ViewModel.Basket;
using ViewModel.Checkout.Payment;
using ViewModel.Checkout.Summary;
using titizOto.App_GlobalResources;
using titizOto.Models;
using ViewModel.Checkout.ErrorProcess;
using ViewModel.Checkout.Complete;
using System.Data.Entity.Validation;
using ViewModel.Shared;


namespace titizOto.Controllers
{
    //[RequireHttps(Order = 1)]
    [cartSummaryBind(Order = 2)]
    [sessionCheckoutControl(Order = 3)]
    public class CheckoutController : DbWithControllerWithMaster
    {
        // Step 0 
        #region Main && CheckoutOption

        public ActionResult Main(int pageId)
        {
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            return Redirect("~/" + checkoutItem.stepLinkList.Where(a => a.step == checkoutStep.registerOptions).First().url);
        }

        [titleDescriptionBinder]
        public ActionResult CheckoutOption(int pageId)
        {
            helperPageRegisterStatu pageHelper = new helperPageRegisterStatu();
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            checkoutItem.clearDataOnStepAndBindCurrentStep(checkoutStep.registerOptions);

            sharedCheckoutItemLoad(pageId, pageHelper, checkoutItem);

            if (checkoutItem.isRegisterOptionsValid())
            {
                checkoutItem.lastSuccessStep = checkoutStep.registerOptions;
                Session["checkoutProcess"] = checkoutItem;
                return redirectToStep(checkoutStep.delivery, checkoutItem);
            }

            Session["checkoutProcess"] = checkoutItem;

            return View(pageHelper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [titleDescriptionBinder]
        public ActionResult CheckoutOption(int pageId, helperPageRegisterStatu pageHelper)
        {
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];

            if (ModelState.IsValid)
            {
                // Go to Delivery
                if (pageHelper.registerStatu == (int)registerOption.guestUser)
                {
                    checkoutItem.registerOption = registerOption.guestUser;
                    checkoutItem.lastSuccessStep = checkoutStep.registerOptions;
                    Session["checkoutProcess"] = checkoutItem;

                    return Redirect("~/" + checkoutItem.stepLinkList.Where(a => a.step == checkoutStep.delivery).First().url);
                }

                // Redirect Login Page
                if (pageHelper.registerStatu == (int)registerOption.registerOrLogin)
                {
                    pageShared ps = new pageShared(db);
                    checkoutItem.registerOption = registerOption.registerOrLogin;

                    Session["redirectPage"] = Request.Url.ToString();
                    Session["checkoutProcess"] = checkoutItem;

                    var loginPage = ps.getPageByType(pageType.registerLogin, langId);
                    if (loginPage == null)
                    {
                        errorSend(new Exception("loginPage gelmemesi, checkout Step 0"), "LoginPage gelmemesi, checkout Step 0", true);
                        return null;
                    }

                    return Redirect("~/" + langCode + "/" + loginPage.url + ".html");
                }
            }


            addErrorTempData(getErrorMessage(getModelStateError(ModelState)));
            return CheckoutOption(pageId);
        }


        #endregion

        // Step 1
        #region Delivery

        [titleDescriptionBinder]
        public ActionResult Delivery(int pageId)
        {
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            checkoutItem.clearDataOnStepAndBindCurrentStep(checkoutStep.delivery);

            // Validation
            var validation = checkoutItem.validationOnCurrentStep(db);
            if (!validation.Item1)
            {
                return redirectToValidation(validation, checkoutItem);
            }

            // Kayıtlı Üye
            if (checkoutItem.cartItem.isRegisteredUser)
            {
                addressShared ads = new addressShared(db);
                userShared us = new userShared(db);

                helperRegisterDelivery helperPage = new helperRegisterDelivery();
                sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);
                helperPage.addressList = ads.getAddressListTemplate(checkoutItem.cartItem.userId).OrderByDescending(a => a.addressId).ToList();
                helperPage.selectedDeliveryAddressId = checkoutItem.deliveryAddressId;
                helperPage.userguid = checkoutItem.cartItem.userGuid;

                Session["checkoutProcess"] = checkoutItem;
                return View("DeliveryRegister", helperPage);

            }
            else // Üye olmadan Ödeme
            {
                helperUnRegisterDelivery helperPage = new helperUnRegisterDelivery();
                sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);

                // Bind AddressItem
                if (checkoutItem.deliveryAddress != null)
                {
                    helperPage.addressItem = checkoutItem.deliveryAddress;
                }
                else
                {
                    helperPage.addressItem = new Models.tbl_address();
                }

                // Bind TrackInfo ( Name, Surname, Email )
                if (checkoutItem.trackInfo != null)
                {
                    helperPage.trackInfoItem = checkoutItem.trackInfo;
                }
                else
                {
                    helperPage.trackInfoItem = new deliveryTrackInfo();
                }

                helperPage.addressItem.isPersonal = true;

                Session["checkoutProcess"] = checkoutItem;
                return View("DeliveryUnRegister", helperPage);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeliveryUnRegister(helperUnRegisterDelivery pageHelper)
        {
            string redirectPage = "";
            string isSuccess = "no";
            string html = "";

            if (ModelState.IsValid)
            {
                checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];

                pageHelper.addressItem.isGuestUser = true;

                checkoutItem.deliveryAddress = pageHelper.addressItem;
                checkoutItem.trackInfo = pageHelper.trackInfoItem;

                checkoutItem.lastSuccessStep = checkoutStep.delivery;

                // Same Billing Address
                if (pageHelper.isSameBillingAddress)
                {
                    checkoutItem.isBillingSameAddress = true;
                    checkoutItem.billingAddress = pageHelper.addressItem;
                    redirectPage = getSiteNameWithoutSlash(Request) + redirecToStepUrlRelative(checkoutStep.cargo, checkoutItem);
                    checkoutItem.lastSuccessStep = checkoutStep.billing;
                }
                else
                {
                    redirectPage = getSiteNameWithoutSlash(Request) + redirecToStepUrlRelative(checkoutStep.billing, checkoutItem);
                }

                isSuccess = "yes";
                Session["checkoutProcess"] = checkoutItem;

            }
            else
            {
                pageHelper.isMessageExist = true;
                pageHelper.message = getErrorMessage(getModelStateError(ModelState), "autoHide");
                html = RenderRazorViewToString("DeliveryUnRegisterModal", pageHelper);
            }

            return Json(new { redirectPage = redirectPage, isSuccess = isSuccess, html = html });
        }

        public JsonResult DeliveryRegisterModal(int pageId, int addressId)
        {
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];

            // Kayıtlı Üye
            if (checkoutItem.cartItem.isRegisteredUser)
            {
                addressShared ads = new addressShared(db);
                userShared us = new userShared(db);
                helperRegisterDelivery helperPage = new helperRegisterDelivery();

                sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);

                helperPage.addressList = ads.getAddressListTemplate(checkoutItem.cartItem.userId).OrderByDescending(a => a.addressId).ToList();
                helperPage.userguid = checkoutItem.cartItem.userGuid;
                helperPage.selectedDeliveryAddressId = addressId;

                string htmlText = RenderRazorViewToString("DeliveryRegisterModal", helperPage);
                return Json(new { html = htmlText }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeliveryRegister(helperRegisterDelivery pageHelper)
        {
            string redirectPage = "";
            string isSuccess = "no";
            string html = "";

            if (ModelState.IsValid)
            {
                checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
                checkoutItem.deliveryAddressId = pageHelper.selectedDeliveryAddressId;
                checkoutItem.lastSuccessStep = checkoutStep.delivery;

                // Same Billing Address
                if (pageHelper.isSameBillingAddress)
                {
                    checkoutItem.isBillingSameAddress = true;
                    checkoutItem.billingAddressId = pageHelper.selectedDeliveryAddressId;
                    redirectPage = getSiteNameWithoutSlash(Request) + redirecToStepUrlRelative(checkoutStep.cargo, checkoutItem);
                    checkoutItem.lastSuccessStep = checkoutStep.billing;
                }
                else
                {
                    redirectPage = getSiteNameWithoutSlash(Request) + redirecToStepUrlRelative(checkoutStep.billing, checkoutItem);
                }

                isSuccess = "yes";
                Session["checkoutProcess"] = checkoutItem;
            }
            else
            {
                pageHelper.isMessageExist = true;
                pageHelper.message = getErrorMessage(getModelStateError(ModelState), "autoHide");
                html = RenderRazorViewToString("DeliveryRegisterModal", pageHelper);
            }

            return Json(new { redirectPage = redirectPage, isSuccess = isSuccess, html = html });

        }

        #endregion

        // Step 2
        #region Billing

        [titleDescriptionBinder]
        public ActionResult Billing(int pageId)
        {
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            checkoutItem.clearDataOnStepAndBindCurrentStep(checkoutStep.billing);

            // Validation
            var validation = checkoutItem.validationOnCurrentStep(db);
            if (!validation.Item1)
            {
                return redirectToValidation(validation, checkoutItem);
            }

            // Kayıtlı Üye
            if (checkoutItem.cartItem.isRegisteredUser)
            {
                addressShared ads = new addressShared(db);
                userShared us = new userShared(db);

                helperRegisterBilling helperPage = new helperRegisterBilling();
                sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);
                helperPage.addressList = ads.getAddressListTemplate(checkoutItem.cartItem.userId).OrderByDescending(a => a.addressId).ToList();
                helperPage.selectedBillingAddressId = checkoutItem.billingAddressId;

                helperPage.userguid = checkoutItem.cartItem.userGuid;

                Session["checkoutProcess"] = checkoutItem;
                return View("BillingRegister", helperPage);
            }
            else // Üye olmadan Ödeme
            {
                helperUnRegisterBilling helperPage = new helperUnRegisterBilling();
                sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);

                if (checkoutItem.billingAddress != null)
                {
                    helperPage.addressItem = checkoutItem.billingAddress;
                }
                else
                {
                    if (checkoutItem.isBillingSameAddress && checkoutItem.deliveryAddress != null)
                    {
                        helperPage.addressItem = checkoutItem.deliveryAddress;
                    }
                    else
                    {
                        helperPage.addressItem = new Models.tbl_address();
                    }
                }

                helperPage.addressItem.isPersonal = true;

                Session["checkoutProcess"] = checkoutItem;
                return View("BillingUnRegister", helperPage);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BillingRegister(helperRegisterBilling pageHelper)
        {
            string redirectPage = "";
            string isSuccess = "no";
            string html = "";

            if (ModelState.IsValid)
            {
                checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
                checkoutItem.billingAddressId = pageHelper.selectedBillingAddressId;
                checkoutItem.lastSuccessStep = checkoutStep.billing;
                redirectPage = getSiteNameWithoutSlash(Request) + redirecToStepUrlRelative(checkoutStep.cargo, checkoutItem);

                isSuccess = "yes";
                Session["checkoutProcess"] = checkoutItem;
            }
            else
            {
                pageHelper.isMessageExist = true;
                pageHelper.message = getErrorMessage(getModelStateError(ModelState), "autoHide");
                html = RenderRazorViewToString("BillingRegisterModal", pageHelper);
            }

            return Json(new { redirectPage = redirectPage, isSuccess = isSuccess, html = html });
        }

        public JsonResult BillingRegisterModal(int pageId, int addressId)
        {
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];


            // Kayıtlı Üye
            if (checkoutItem.cartItem.isRegisteredUser)
            {
                addressShared ads = new addressShared(db);
                userShared us = new userShared(db);

                helperRegisterBilling helperPage = new helperRegisterBilling();
                sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);
                helperPage.addressList = ads.getAddressListTemplate(checkoutItem.cartItem.userId).OrderByDescending(a => a.addressId).ToList();
                helperPage.selectedBillingAddressId = addressId;
                helperPage.userguid = checkoutItem.cartItem.userGuid;


                string htmlText = RenderRazorViewToString("BillingRegisterModal", helperPage);

                return Json(new { html = htmlText }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BillingUnRegister(helperUnRegisterBilling pageHelper)
        {
            string redirectPage = "";
            string isSuccess = "no";
            string html = "";

            if (ModelState.IsValid)
            {
                checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];

                pageHelper.addressItem.isGuestUser = true;
                checkoutItem.billingAddress = pageHelper.addressItem;
                checkoutItem.lastSuccessStep = checkoutStep.billing;
                redirectPage = getSiteNameWithoutSlash(Request) + redirecToStepUrlRelative(checkoutStep.cargo, checkoutItem);

                isSuccess = "yes";
                Session["checkoutProcess"] = checkoutItem;

            }
            else
            {
                pageHelper.isMessageExist = true;
                pageHelper.message = getErrorMessage(getModelStateError(ModelState), "autoHide");
                html = RenderRazorViewToString("BillingUnRegisterModal", pageHelper);
            }

            return Json(new { redirectPage = redirectPage, isSuccess = isSuccess, html = html });

        }

        #endregion

        // Step 3
        #region Cargo

        [titleDescriptionBinder]
        public ActionResult Cargo(int pageId)
        {
            checkoutShared cs = new checkoutShared(db);
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            checkoutItem.clearDataOnStepAndBindCurrentStep(checkoutStep.cargo);

            // Validation
            var validation = checkoutItem.validationOnCurrentStep(db);
            if (!validation.Item1)
            {
                return redirectToValidation(validation, checkoutItem);
            }

            helperCheckoutCargo helperPage = new helperCheckoutCargo();
            sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);

            // get Basket Content => Price , Discount 
            var productPriceItem = cs.getProductPrice(checkoutItem.cartItem, langId, langCode, mainPath, false);
            if (!productPriceItem.Item1)
            {
                return returnBasketMainPage(null);
            }

            var basketTotal = productPriceItem.Item2;

            // get Cargo List
            helperPage.cargoList = cs.getCargoItemList(basketTotal, langId, "en-US");
            helperPage.selectedCargoId = checkoutItem.cargoId;

            Session["checkoutProcess"] = checkoutItem;
            return View(helperPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cargo(int pageId, helperCheckoutCargo helperPage)
        {
            basketShared bs = new basketShared(db);
            checkoutShared cs = new checkoutShared(db);
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];

            if (ModelState.IsValid)
            {
                checkoutItem.cargoId = helperPage.selectedCargoId;
                checkoutItem.lastSuccessStep = checkoutStep.cargo;
                Session["checkoutProcess"] = checkoutItem;
                return redirectToStep(checkoutStep.payment, checkoutItem);
            }
            else
            {

                addErrorTempData(getErrorMessage(getModelStateError(ModelState), "autoHide"));
                return Cargo(pageId);
            }
        }

        #endregion

        // Step 4
        #region Payment

        [titleDescriptionBinder]
        public ActionResult PaymentOption(int pageId)
        {
            checkoutShared cs = new checkoutShared(db);
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            checkoutItem.clearDataOnStepAndBindCurrentStep(checkoutStep.payment);

            // Validation
            var validation = checkoutItem.validationOnCurrentStep(db);
            if (!validation.Item1)
            {
                return redirectToValidation(validation, checkoutItem);
            }

            helperPaymentOption helperPage = new helperPaymentOption();
            sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);
            helperPage.paymentOptionId = (int)checkoutItem.paymentOptionChoose;

            transferDiscount item = cs.getTransferInfo(langId);
            helperPage.transferDiscountText = cs.getTransferInfoText(item, "TL");


            // Payment Option Visiable From Settings
            var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();
            if (settingItem.isCrediCardEnable.HasValue && settingItem.isCrediCardEnable.Value)
            {
                helperPage.isCredicardVisiable = true;
            }

            if (settingItem.isTransferEnable.HasValue && settingItem.isTransferEnable.Value)
            {
                helperPage.isTransferVisiable = true;
            }

            // If Is Test Account EveryPayment Actice
            #region Test Account

            if (!string.IsNullOrWhiteSpace(settingItem.testAccountEmail))
            {
                var testAccountList = settingItem.testAccountEmail.Split(',').ToList();

                if (checkoutItem.cartItem.isRegisteredUser)
                {
                    var userItem = db.tbl_user.Where(a => a.userId == checkoutItem.cartItem.userId).First();

                    if (testAccountList.Contains(userItem.email))
                    {
                        helperPage.isCredicardVisiable = true;
                        helperPage.isTransferVisiable = true;
                    }
                }
                else
                {

                    if (testAccountList.Contains(checkoutItem.trackInfo.email))
                    {
                        helperPage.isCredicardVisiable = true;
                        helperPage.isTransferVisiable = true;
                    }
                }
            }

            #endregion


            return View(helperPage);
        }

        public ActionResult PaymentRedirect(int paymentOptionId)
        {
            paymentOption val = (paymentOption)paymentOptionId;
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];

            switch (val)
            {
                case paymentOption.transfer:
                    checkoutItem.paymentOptionChoose = paymentOption.transfer;
                    Session["checkoutProcess"] = checkoutItem;
                    return Transfer(true);


                case paymentOption.creditCard:
                    checkoutItem.paymentOptionChoose = paymentOption.creditCard;
                    Session["checkoutProcess"] = checkoutItem;
                    return Credit(true);
            }

            return null;
        }

        public ActionResult Transfer(bool isModal)
        {
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            checkoutShared cs = new checkoutShared(db);
            transferInfo helperPage = new transferInfo();

            helperPage.eftList = cs.getEftList(langId);
            helperPage.selectedTransferId = checkoutItem.transferInfo.selectedTransferId;

            // Bind Post Error
            if (TempData["errorMessage"] != null)
            {
                helperPage.isMessageExist = true;
                helperPage.message = TempData["errorMessage"].ToString();
            }

            if (isModal)
            {
                string html = RenderRazorViewToString("Transfer", helperPage);
                return Json(new { html = html }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View(helperPage);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer(int selectedTransferId)
        {
            if (ModelState.IsValid)
            {
                checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
                checkoutItem.transferInfo.selectedTransferId = selectedTransferId;
                checkoutItem.lastSuccessStep = checkoutStep.payment;
                Session["checkoutProcess"] = checkoutItem;
                return Json(new { redirectPage = redirectToStepFullSiteName(checkoutStep.summary, checkoutItem), isSuccess = "yes" }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                addErrorTempData(getErrorMessage(getModelStateError(ModelState), "autoHide"));
                return Transfer(true);

            }
        }

        public ActionResult Credit(bool isModal)
        {

            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            cardInfo helperPage = checkoutItem.cardInfo;
            checkoutShared cs = new checkoutShared(db);
            basketShared bs = new basketShared(db);

            // Bind Post Error
            if (TempData["errorMessage"] != null)
            {
                helperPage.isErrorExist = true;
                helperPage.message = TempData["errorMessage"].ToString();
            }

            // Product Price
            var productPriceItem = cs.getProductPrice(checkoutItem.cartItem, langId, langCode, mainPath, false);
            if (!productPriceItem.Item1)
            {
                // Redirect  Error
                #region Error

                if (isModal)
                {
                    string redirectPage = getSiteNameWithoutSlash(Request) + returnBasketMainPageRelativeUrl(null);
                    return Json(new { redirectPage = redirectPage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return returnBasketMainPage(null);
                }

                #endregion
            }

            var productPrice = productPriceItem.Item2;


            // Cargo Price
            var cargoPrice = cs.getCargoPriceByCargoId(checkoutItem.cargoId, productPrice, langId);

            if (cargoPrice < 0)
            {
                // Error On Cargo
                #region Error
                {
                    if (isModal)
                    {

                        return Json(new { redirectPage = redirectToStepFullSiteName(checkoutStep.cargo, checkoutItem) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return returnBasketMainPage(null);
                    }

                }
                #endregion
            }


            var cargoAndProductPrice = productPrice + cargoPrice;

            helperPage.totalPriceStr = cargoAndProductPrice.ToString("F2", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

            if (isModal)
            {
                string html = RenderRazorViewToString("Credit", helperPage);
                return Json(new { html = html }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View(helperPage);
            }
        }

        public ActionResult CreditOption(bool isModal, string creditCard)
        {
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            checkoutShared cs = new checkoutShared(db);
            cardOption helperPage = new cardOption();

            // Product Price
            var productPriceItem = cs.getProductPrice(checkoutItem.cartItem, langId, langCode, mainPath, false);
            if (!productPriceItem.Item1)
            {
                // Redirect  Error
                #region Error

                if (isModal)
                {
                    string redirectPage = getSiteNameWithoutSlash(Request) + returnBasketMainPageRelativeUrl(null);
                    return Json(new { redirectPage = redirectPage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return returnBasketMainPage(null);
                }

                #endregion
            }

            var productPrice = productPriceItem.Item2;


            // Cargo Price
            var cargoPrice = cs.getCargoPriceByCargoId(checkoutItem.cargoId, productPrice, langId);

            if (cargoPrice < 0)
            {
                // Error On Cargo
                #region Error
                {
                    if (isModal)
                    {

                        return Json(new { redirectPage = redirectToStepFullSiteName(checkoutStep.cargo, checkoutItem) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return returnBasketMainPage(null);
                    }

                }
                #endregion
            }


            var cargoAndProductPrice = productPrice + cargoPrice;


            //  get Option List
            if (cs.isCreditCardValid(creditCard))
            {
                helperPage.optionList = cs.getCardOptionList(langId, cargoAndProductPrice, creditCard, HelperSite.Pos.posType.standart, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), "TL");

                if (helperPage.optionList == null)
                {
                    errorSend(new Exception("mainPosNull geliyor"), "Pos atanmamış");
                    helperPage.isErrorExist = true;
                }
            }

            helperPage.creditOptionId = checkoutItem.cardInfo.cardOption.creditOptionId;

            if (isModal)
            {
                string html = RenderRazorViewToString("CreditOption", helperPage);
                return Json(new { html = html }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View(helperPage);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Credit(cardInfo helper, int creditOptionId)
        {
            helper.cardOption = new cardOption();
            helper.cardOption.creditOptionId = creditOptionId;
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];

            if (ModelState.IsValid)
            {
                helper.isErrorExist = false;
                helper.message = null;
                checkoutItem.cardInfo = helper;
                checkoutItem.lastSuccessStep = checkoutStep.payment;
                Session["checkoutProcess"] = checkoutItem;
                return Json(new { redirectPage = redirectToStepFullSiteName(checkoutStep.summary, checkoutItem), isSuccess = "yes" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                addErrorTempData(getErrorMessage(getModelStateError(ModelState), "autoHide"));
                return Credit(true);
            }
        }

        #endregion

        // Step 5
        #region Summary

        [titleDescriptionBinder]
        public ActionResult Summary(int pageId)
        {
            checkoutShared cs = new checkoutShared(db);
            basketShared bs = new basketShared(db);
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            checkoutItem.clearDataOnStepAndBindCurrentStep(checkoutStep.summary);

            // Validation
            var validation = checkoutItem.validationOnCurrentStep(db);
            if (!validation.Item1)
            {
                return redirectToValidation(validation, checkoutItem);
            }

            helperSummaryPage helperPage = new helperSummaryPage();
            sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);


            #region OrderSummary Validation
            var orderSummaryItem = getOrderSummary(checkoutItem, cs, bs);

            if (!orderSummaryItem.Item1)
            {
                return orderSummaryItem.Item3;
            }

            helperPage.orderSummary = orderSummaryItem.Item2;

            #endregion

            #region Agreement


            orderInfo orderInfoItem = cs.getOrderInfoByCheckoutProcess(checkoutItem, helperPage.orderSummary, this, BasketHtmlType.agreement, AddressHtmlType.agreement, TransferHtmlType.mail, langId);

            helperPage.salesAgreement = cs.getSalesAgreement(orderInfoItem.customerNameSurname, orderInfoItem.deliveryHtml, orderInfoItem.customerPhone, orderInfoItem.customerEmail, orderInfoItem.customerBasket, orderInfoItem.orderDate);

            helperPage.preSalesAgreement = cs.getPreSalesAgreement(orderInfoItem.customerNameSurname, orderInfoItem.deliveryHtml, orderInfoItem.customerPhone, orderInfoItem.customerEmail, orderInfoItem.customerBasket, orderInfoItem.orderDate);

            #endregion

            helperPage.orderNote = checkoutItem.orderNote;

            return View(helperPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Summary(int pageId, bool isAgreementChecked, bool isPreSalesAgreementChecked, string orderNote)
        {
            addressShared ads = new addressShared(db);
            checkoutShared cs = new checkoutShared(db);
            basketShared bs = new basketShared(db);
            mailShared ms = new mailShared(db, langId);
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            checkoutItem.clearDataOnStepAndBindCurrentStep(checkoutStep.summary);


            // Process Erorr && Redirect Page
            Tuple<bool, ActionResult> summaryError = new Tuple<bool, ActionResult>(false, null);


            // Last Validation
            var validation = checkoutItem.validationOnCurrentStep(db);
            if (!validation.Item1)
            {
                return redirectToValidation(validation, checkoutItem);
            }

            checkoutItem.orderNote = orderNote;

            if (ModelState.IsValid)
            {
                #region Get Summary

                helperSummaryPage helperPage = new helperSummaryPage();
                sharedCheckoutItemLoad(pageId, helperPage, checkoutItem);

                #region OrderSummary Validation
                var orderSummaryItem = getOrderSummary(checkoutItem, cs, bs);

                if (!orderSummaryItem.Item1)
                {
                    return orderSummaryItem.Item3;
                }

                helperPage.orderSummary = orderSummaryItem.Item2;

                #endregion

                #region Agreement

                orderInfo orderInfoItem = cs.getOrderInfoByCheckoutProcess(checkoutItem, helperPage.orderSummary, this, BasketHtmlType.agreement, AddressHtmlType.agreement, TransferHtmlType.mail, langId);

                helperPage.salesAgreement = cs.getSalesAgreement(orderInfoItem.customerNameSurname, orderInfoItem.deliveryHtml, orderInfoItem.customerPhone, orderInfoItem.customerEmail, orderInfoItem.customerBasket, orderInfoItem.orderDate);

                helperPage.preSalesAgreement = cs.getPreSalesAgreement(orderInfoItem.customerNameSurname, orderInfoItem.deliveryHtml, orderInfoItem.customerPhone, orderInfoItem.customerEmail, orderInfoItem.customerBasket, orderInfoItem.orderDate);

                #endregion

                #endregion

                var orderItem = new tbl_order();

                #region Shared

                orderItem.orderNo = cs.getOrderNo();
                orderItem.cargoId = checkoutItem.cargoId;
                orderItem.paymentTypeId = (int)checkoutItem.paymentOptionChoose;
                orderItem.cargoPrice = helperPage.orderSummary.cargoPrice;
                orderItem.orderMailStatu = 0;
                orderItem.totalProductPrice = helperPage.orderSummary.basketItem.totalPriceDec;
                orderItem.discountAmount = helperPage.orderSummary.basketItem.discountTotalAmount;
                orderItem.discountCode = helperPage.orderSummary.basketItem.discountCodeString;
                orderItem.totalCheckoutPrice = helperPage.orderSummary.allTotalPrice;
                orderItem.createDate = DateTime.Now;
                orderItem.salesAgreement = helperPage.salesAgreement;
                orderItem.preSalesAgreement = helperPage.preSalesAgreement;
                orderItem.orderGuid = Guid.NewGuid().ToString();
                orderItem.orderNote = orderNote;
                orderItem.isCargoOnCustomer = helperPage.orderSummary.isCargoOnCustomer;

                #endregion

                // Get Payment &&  Generate Order Item
                switch (checkoutItem.paymentOptionChoose)
                {

                    case paymentOption.transfer:

                        // Shared Order Parameter On Transfer
                        #region Transfer Shared

                        // to DO 
                        orderItem.orderStatu = (int)orderStatu.waitPayment;
                        orderItem.paymentTypeId = (int)paymentOption.transfer;
                        orderItem.additionalPrice = 0;
                        orderItem.creditPaymentCount = 0;
                        orderItem.eftId = checkoutItem.transferInfo.selectedTransferId;
                        orderItem.transferDiscount = helperPage.orderSummary.transferDiscount;

                        #endregion


                        if (checkoutItem.cartItem.isRegisteredUser)
                        {
                            #region Register
                            orderItem.userId = checkoutItem.cartItem.userId;
                            orderItem.trackInfoId = 0;
                            orderItem.deliveryAddressId = checkoutItem.deliveryAddressId;
                            orderItem.deliveryAddressObj = serializeObject(ads.getAddressById(checkoutItem.deliveryAddressId));
                            orderItem.billingAddressId = checkoutItem.billingAddressId;
                            orderItem.billingAddressObj = serializeObject(ads.getAddressById(checkoutItem.billingAddressId));

                            orderItem.isRegisteredOrder = true;

                            #endregion
                        }
                        else
                        {
                            #region Unregister
                            orderItem.userId = 0;

                            // AddTrackInfo With Try
                            #region  AddTrackInfo
                            try
                            {
                                tbl_trackInfo addedTrackInfo = cs.addTrackInfo(checkoutItem.trackInfo.name, checkoutItem.trackInfo.surname, checkoutItem.trackInfo.email);
                                orderItem.trackInfoId = addedTrackInfo.trackInfoId;
                            }
                            catch (Exception ex)
                            {
                                errorSend(ex, "Add TrackInfo Checkout -- " + serializeObject(checkoutItem.trackInfo), true);

                                summaryError = getErrorOnSummary(summaryActionResult.trackInfoAddError, checkoutItem);
                                if (summaryError.Item1)
                                {
                                    return summaryError.Item2;
                                }
                            }

                            #endregion

                            // Add Delivery Address
                            #region  AddAddress
                            try
                            {
                                checkoutItem.deliveryAddress.isGuestUser = true;
                                checkoutItem.deliveryAddress.userId = orderItem.trackInfoId;
                                checkoutItem.deliveryAddress.statu = true;

                                var addressItem = ads.addAddress(checkoutItem.deliveryAddress);
                                orderItem.deliveryAddressId = addressItem.addressId;
                                orderItem.deliveryAddressObj = serializeObject(addressItem);
                            }
                            catch (Exception ex)
                            {

                                errorSend(ex, "Add Delivery Address Checkout" + serializeObject(checkoutItem.deliveryAddress), true);

                                summaryError = getErrorOnSummary(summaryActionResult.deliveryAddError, checkoutItem);
                                if (summaryError.Item1)
                                {
                                    return summaryError.Item2;
                                }
                            }

                            #endregion

                            // Add Billing Address
                            #region  AddAddress
                            try
                            {
                                checkoutItem.deliveryAddress.isGuestUser = true;
                                checkoutItem.deliveryAddress.userId = orderItem.trackInfoId;
                                checkoutItem.deliveryAddress.statu = true;

                                var addressItem = ads.addAddress(checkoutItem.billingAddress);
                                orderItem.billingAddressId = addressItem.addressId;
                                orderItem.billingAddressObj = serializeObject(addressItem);
                            }
                            catch (Exception ex)
                            {

                                errorSend(ex, "Add Billing Address Checkout" + serializeObject(checkoutItem.billingAddress), true);

                                summaryError = getErrorOnSummary(summaryActionResult.billingAddError, checkoutItem);
                                if (summaryError.Item1)
                                {
                                    return summaryError.Item2;
                                }
                            }

                            #endregion


                            #endregion
                        }

                        break;

                    case paymentOption.creditCard:


                        break;
                }


                // Add Order 
                #region Add Order

                try
                {
                    db.tbl_order.Add(orderItem);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    errorSend(ex, "Order Ekleme -- " + serializeObject(orderItem), true);
                    summaryError = getErrorOnSummary(summaryActionResult.orderAddError, checkoutItem);
                    if (summaryError.Item1)
                    {
                        return summaryError.Item2;
                    }
                }

                #endregion

                // Add Order Detail
                #region Add OrderDetail

                foreach (var item in helperPage.orderSummary.basketItem.basketList)
                {
                    var orderDetailItem = new tbl_orderDetail();
                    orderDetailItem.orderId = orderItem.orderId;
                    orderDetailItem.productId = item.productId;
                    orderDetailItem.productPrice = item.productPriceDec;
                    orderDetailItem.productTotalPrice = item.productTotalPriceDec;
                    orderDetailItem.quantity = item.quantity;
                    orderDetailItem.optionList = item.optionCode;
                    orderDetailItem.photo = item.photo;
                    orderDetailItem.nameWithOption = item.productDescriptionWithOptionItem;

                    db.tbl_orderDetail.Add(orderDetailItem);
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    errorSend(ex, "Order Detail Ekleme -- " + serializeObject(helperPage.orderSummary.basketItem.basketList), true);
                    summaryError = getErrorOnSummary(summaryActionResult.orderDetailAddError, checkoutItem);
                    if (summaryError.Item1)
                    {
                        return summaryError.Item2;
                    }
                }

                #endregion

                // Is Test Account 
                #region Is Test Account

                bool isTesterAccount = false;
                var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();

                if (orderInfoItem.customerEmail == settingItem.testAccountEmail)
                {
                    isTesterAccount = true;
                }

                #endregion

                // Send Mail Type
                #region Send Mail

                orderInfoItem = cs.getOrderInfoByCheckoutProcess(checkoutItem, helperPage.orderSummary, this, BasketHtmlType.mail, AddressHtmlType.mail, TransferHtmlType.mail, langId);

                switch (checkoutItem.paymentOptionChoose)
                {

                    case paymentOption.transfer:

                        var orderTransferMail = ms.getTransferMailContent(orderInfoItem.customerNameSurname, orderItem.orderNo, orderInfoItem.transferAccountHtml, orderInfoItem.deliveryHtml, orderInfoItem.billingHtml, orderInfoItem.customerBasket);

                        // Send Mail
                        try
                        {
                            if (!isTesterAccount)
                            {
                                mailSend(orderTransferMail.Item1, orderTransferMail.Item2);
                            }

                            mailSend(orderInfoItem.customerEmail, orderTransferMail.Item1, orderTransferMail.Item2);
                            mailSend("erkan.aslanel@gmail.com", orderTransferMail.Item1, orderTransferMail.Item2);

                        }
                        catch (Exception ex)
                        {

                            errorSend(ex, "order Transfer Mail Send", true);
                        }

                        break;

                    case paymentOption.creditCard:


                        break;

                }

                #endregion

                // updateOrderStock && minStockSend
                #region updateOrderStock && minStockSend

                List<Tuple<int, string>> minStockList = new List<Tuple<int, string>>();

                foreach (var item in helperPage.orderSummary.basketItem.basketList)
                {
                    var stockItem = db.tbl_product.Include("tbl_stock").Where(a => a.productId == item.productId).SelectMany(a => a.tbl_stock).ToList().Where(a => a.optionList == item.optionCode).FirstOrDefault();

                    if (stockItem != null)
                    {

                        var updatedStock = db.tbl_stock.Where(a => a.stockId == stockItem.stockId).FirstOrDefault();
                        updatedStock.stockCount = updatedStock.stockCount - item.quantity;

                        if (updatedStock.stockCount <= updatedStock.minCount)
                        {
                            minStockList.Add(new Tuple<int, string>(item.productId, item.productDescriptionWithOptionItem));
                        }
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    errorSend(ex, "stockUpdate", true);
                }

                if (minStockList.Count > 0)
                {
                    string minStockHtml = RenderRazorViewToString("MinStockMail", minStockList);
                    var minStockMailItem = ms.getMinStockMail(minStockHtml);

                    try
                    {
                        mailSend(minStockMailItem.Item1, minStockMailItem.Item2);
                    }
                    catch (Exception ex)
                    {

                        errorSend(ex, "minStock Send Mail", true);
                    }
                }

                #endregion

                // Delete Current Basket 
                var basketContent = bs.getBasketContent(checkoutItem.cartItem, langId, langCode, mainPath, false);
                foreach (var item in basketContent.Item1)
                {
                    bs.deleteBasketById(item.basketId);
                }

                // Redirect Order Complete 
                return redirectToStep(checkoutStep.complete, checkoutItem, "?orderGuid=" + orderItem.orderGuid);
            }
            else
            {
                addErrorTempData(getErrorMessage(getModelStateError(ModelState), "autoHide"));
                return Summary(pageId);
            }
        }

        public Tuple<bool, ActionResult> getErrorOnSummary(summaryActionResult result, checkoutProcess checkoutItem)
        {
            switch (result)
            {
                case summaryActionResult.trackInfoAddError:
                case summaryActionResult.deliveryAddError:
                case summaryActionResult.billingAddError:
                case summaryActionResult.orderAddError:
                case summaryActionResult.orderDetailAddError:

                    addErrorTempData(getErrorMessage(App_GlobalResources.lang.orderErrorOnSaveOrder));
                    return new Tuple<bool, ActionResult>(true, redirectToStep(checkoutStep.error, checkoutItem));


                case summaryActionResult.posCodeError:

                    addErrorTempData(getErrorMessage(App_GlobalResources.lang.orderErrorOnPayment));
                    return new Tuple<bool, ActionResult>(true, redirectToStep(checkoutStep.summary, checkoutItem));

                case summaryActionResult.posError:

                    addErrorTempData(getErrorMessage(App_GlobalResources.lang.orderErrorPaymentInfoWrong));
                    return new Tuple<bool, ActionResult>(true, redirectToStep(checkoutStep.payment, checkoutItem));

            }

            return new Tuple<bool, ActionResult>(false, null);

        }


        #endregion

        // Step 6
        #region Complete

        [titleDescriptionBinder]
        public ActionResult Complete(int pageId)
        {
            helperComplete pageHelper = new helperComplete();
            orderShared os = new orderShared(db);
            checkoutShared cs = new checkoutShared(db);
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];

            if (Request.QueryString["orderGuid"] != null)
            {
                string orderNo = "";
                string orderDetailLink = "";

                sharedCheckoutItemLoad(pageId, pageHelper, checkoutItem);

                Session["checkoutProcess"] = null;


                var orderItem = os.getOrderByGuid(Request.QueryString["orderGuid"]);

                if (orderItem != null)
                {
                    // orderNo
                    orderNo = orderItem.orderNo;
                    pageHelper.detail = pageHelper.detail.Replace("[order_no]", orderNo);

                    // orderDetail Link
                    orderDetailLink = os.getOrderDetailLink(orderItem.orderGuid, langId, langCode);
                    pageHelper.detail = pageHelper.detail.Replace("[orderNoDetail]", Url.Content("~/" + orderDetailLink));

                    if (orderItem.paymentTypeId == (int)paymentOption.transfer)
                    {
                        pageHelper.isTransferOrder = true;
                        pageHelper.transferAcountHtml = cs.getTransferInfoHtml(orderItem.eftId, langId, this, TransferHtmlType.orderDetail);
                    }

                    return View(pageHelper);
                }
            }

            // Return Basket
            return returnBasketMainPage(null);


        }


        #endregion

        // Step 7 
        #region ErrorProcess

        [titleDescriptionBinder]
        public ActionResult ErrorProcess(int pageId)
        {
            helperErrorProcess pageHelper = new helperErrorProcess();
            checkoutProcess checkoutItem = (checkoutProcess)Session["checkoutProcess"];
            sharedCheckoutItemLoad(pageId, pageHelper, checkoutItem);

            checkoutItem.cardInfo.creditCard = "";
            checkoutItem.cardInfo.cvv = 0;


            if (TempData["errorMessage"] != null)
            {
                pageHelper.message = TempData["errorMessage"].ToString();

                // Log Error
                errorSend(new Exception("ErrorProcess"), serializeObject(checkoutItem));

                TempData.Remove("errorMessage");
                Session["checkoutProcess"] = null;
            }

            return View(pageHelper);

        }

        #endregion


        #region helper

        public Tuple<bool, orderSummary, RedirectResult> getOrderSummary(checkoutProcess checkoutItem, checkoutShared cs, basketShared bs)
        {
            orderSummary helperItem = new orderSummary();

            #region Basket

            var basketContentItem = bs.getBasketHelperWithProductAndDiscount(checkoutItem.cartItem, langId, langCode, mainPath, false);
            if (basketContentItem.Item2 == basketActionResult.redirect)
            {
                return new Tuple<bool, orderSummary, RedirectResult>(false, null, returnBasketMainPage(null));
            }

            helperItem.basketItem = basketContentItem.Item1;

            // Product Price
            var productPrice = helperItem.basketItem.totalPriceDec;

            #endregion

            #region Cargo

            // Cargo Price
            decimal cargoPrice = 0;
            var cargoList = cs.getCargoItemList(helperItem.basketItem.totalPriceDec, langId, "en-US");
            var cargoItem = cargoList.Where(a => a.cargoId == checkoutItem.cargoId).FirstOrDefault();
            if (cargoItem != null)
            {
                if (cargoItem.isCargoOnCustomer)
                {
                    helperItem.cargoPriceStr = lang.checkoutCargoOnCustomer;
                    helperItem.isCargoOnCustomer = true;
                }
                else
                {
                    helperItem.cargoPriceStr = cargoItem.price.ToString("F2", helperItem.basketItem.priceStringFormat);
                    cargoPrice = cargoItem.price;

                }
            }
            else
            {

                return new Tuple<bool, orderSummary, RedirectResult>(false, null, redirectToStep(checkoutStep.cargo, checkoutItem));
            }

            #endregion

            #region Basket & Payment Option

            decimal allPriceTotal = 0;
            decimal additionalPrice = 0;

            helperItem.paymentOptionChoose = checkoutItem.paymentOptionChoose;

            // getPayment Type 
            switch (checkoutItem.paymentOptionChoose)
            {
                case paymentOption.noAnswer:

                    redirectToStep(checkoutStep.payment, checkoutItem);
                    break;
                case paymentOption.transfer:

                    helperItem.paymentOptionChooseStr = lang.checkoutTransfer;
                    allPriceTotal = cargoPrice + productPrice;
                    checkoutItem.transferInfo.transferDiscount = cs.getTransferInfo(langId);
                    if (checkoutItem.transferInfo.transferDiscount.isDiscountExist)
                    {
                        helperItem.isTransferDiscountExist = true;
                        helperItem.transferDiscount = checkoutItem.transferInfo.transferDiscount.calcDiscountAmount(allPriceTotal);
                        helperItem.transferDiscountStr = helperItem.transferDiscount.ToString("F2", helperItem.basketItem.priceStringFormat) + " TL";
                        allPriceTotal = allPriceTotal - helperItem.transferDiscount;
                    }

                    break;
                case paymentOption.creditCard:

                    helperItem.paymentOptionChooseStr = lang.checkoutCrediCard;
                    var productAndCargoPrice = cargoPrice + productPrice;

                    // Has Taksit
                    if (checkoutItem.cardInfo.cardOption.creditOptionId != 0)
                    {
                        var optionItem = cs.getCardOptionList(langId, productAndCargoPrice, checkoutItem.cardInfo.creditCard, HelperSite.Pos.posType.standart, helperItem.basketItem.priceStringFormat, "TL").Where(a => a.bankPosOptionId == checkoutItem.cardInfo.cardOption.creditOptionId).FirstOrDefault();
                        if (optionItem != null)
                        {
                            allPriceTotal = optionItem.totalPrice;
                            additionalPrice = allPriceTotal - (cargoPrice + productPrice);
                            helperItem.paymentOptionChooseStr = helperItem.paymentOptionChooseStr + "(" + optionItem.monthStr + lang.checkoutInstallment + ")";
                        }
                        else
                        {

                            return new Tuple<bool, orderSummary, RedirectResult>(false, null, redirectToStep(checkoutStep.payment, checkoutItem));
                        }
                    }
                    else // No Taksit
                    {
                        if (checkoutItem.cardInfo.cardOption.creditOptionId == -1) // Not Choose Return
                        {
                            return new Tuple<bool, orderSummary, RedirectResult>(false, null, redirectToStep(checkoutStep.payment, checkoutItem));
                        }
                        else
                        {
                            helperItem.paymentOptionChooseStr = helperItem.paymentOptionChooseStr + "(" + lang.checkoutCash + ")";
                            allPriceTotal = cargoPrice + productPrice;
                        }
                    }


                    break;
                default:
                    return new Tuple<bool, orderSummary, RedirectResult>(false, null, redirectToStep(checkoutStep.payment, checkoutItem));


            }

            helperItem.productPrice = productPrice;
            helperItem.productPriceStr = productPrice.ToString("F2", helperItem.basketItem.priceStringFormat) + " TL";
            helperItem.cargoPrice = cargoPrice;
            helperItem.additionalPrice = additionalPrice;
            helperItem.allTotalPrice = allPriceTotal;

            helperItem.allTotalPriceStr = allPriceTotal.ToString("F2", helperItem.basketItem.priceStringFormat) + " TL";
            helperItem.additionalPriceStr = additionalPrice.ToString("F2", helperItem.basketItem.priceStringFormat) + " TL";


            return new Tuple<bool, orderSummary, RedirectResult>(true, helperItem, null);

            #endregion

        }


        public void sharedCheckoutItemLoad(int pageId, helperCheckoutShared pageHelper, checkoutProcess checkoutItem)
        {
            pageShared ps = new pageShared(db);

            var pageItem = ps.getPageById(pageId);
            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.detail = pageItem.detail;
            pageHelper.stepLinkList = checkoutItem.stepLinkList;
            pageHelper.activeStep = checkoutItem.currentStep;
            checkoutItem.lastSuccessStep = ((checkoutStep)((int)checkoutItem.currentStep - 1));

            if (TempData["errorMessage"] != null)
            {
                pageHelper.isMessageExist = true;
                pageHelper.message = TempData["errorMessage"].ToString();
            }
        }

        public void addErrorTempData(string error)
        {
            TempData["errorMessage"] = error;
        }

        public RedirectResult redirectToValidation(Tuple<bool, string, checkoutStep> validation, checkoutProcess checkoutItem)
        {
            // Error Message
            if (validation.Item2 != null)
            {
                return redirectToStep(validation.Item3, checkoutItem, "?requiredMessage=" + validation.Item2);
            }
            else
            {
                return redirectToStep(validation.Item3, checkoutItem);
            }
        }

        public RedirectResult redirectToStep(checkoutStep step, checkoutProcess checkoutItem, string additionalQuery)
        {
            var redirectPageUrl = redirecToStepUrlRelative(step, checkoutItem);

            if (redirectPageUrl != null)
            {
                return Redirect(redirectPageUrl + additionalQuery);
            }

            return null;
        }

        public RedirectResult redirectToStep(checkoutStep step, checkoutProcess checkoutItem)
        {
            return redirectToStep(step, checkoutItem, null);

        }

        public string redirecToStepUrlRelative(checkoutStep step, checkoutProcess checkoutItem)
        {
            if (step == checkoutStep.none)
            {
                return returnBasketMainPageRelativeUrl("update=onCheckout");
            }


            var redirectPage = checkoutItem.stepLinkList.Where(a => a.step == step).FirstOrDefault();

            if (redirectPage != null)
            {
                return Url.Content("~/" + redirectPage.url);
            }
            else
            {

                errorSend(new Exception("stepLinkList düzgün gelmiyor."), "", true);
                return null;
            }
        }

        public string redirectToStepFullSiteName(checkoutStep step, checkoutProcess checkoutItem)
        {
            return getSiteNameWithoutSlash(Request) + redirecToStepUrlRelative(step, checkoutItem);

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

        public string returnBasketMainPageRelativeUrl(string query)
        {
            pageShared ps = new pageShared(db);

            var basketPage = ps.getPageByType(pageType.basket, langId);
            if (basketPage != null)
            {
                if (query != null)
                {
                    return Url.Content("~/" + langCode + "/" + basketPage.url + ".html" + "?" + query);
                }
                else
                {
                    return Url.Content("~/" + langCode + "/" + basketPage.url + ".html");
                }

            }

            else
            {
                return Url.Content("~/");
            }

        }

        #endregion
    }
}
