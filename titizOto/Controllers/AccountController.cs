using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.Attribute;
using HelperSite.DbController;
using HelperSite.Shared;
using titizOto.Models;
using ViewModel.Shared;
using ViewModel.Account.Dashboard;
using ViewModel.Account.Order;
using ViewModel.Account.UserInfo;
using ViewModel.Account.Password;
using ViewModel.Account.Address;
using ViewModel.Account.Discount;
using System.Text.RegularExpressions;
using titizOto.App_GlobalResources;
using System.Globalization;

namespace titizOto.Controllers
{
    public class AccountController : DbWithControllerWithMaster
    {
        #region Dashboard && Main


        [userTypeControl(userType.normalMember, userType.facebookMember)]
        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult Dashboard(int pageId)
        {
            topCart cartItem = (topCart)ViewData["topCart"];
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);
            addressShared ash = new addressShared(db);

            var pageItem = ps.getPageById(pageId);
            var mainAccountPage = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();

            helperDashboard pageHelper = new helperDashboard();

            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.setTitle(pageItem.name);
            pageHelper.detail = pageItem.detail;

            pageHelper.leftMenuList = generateLeftMenu(mainAccountPage, pageItem.url);
            pageHelper.breadCrumbItem = getBreadCrumbTwoPage(mainAccountPage.name, mainAccountPage.url, pageItem.name, pageItem.url);



            // User Info
            var userItem = us.getUserById(cartItem.userId);
            pageHelper.email = userItem.email;
            pageHelper.nameSurname = userItem.name + " " + userItem.surname;
            var newsletterPage = ps.getPageByType(pageType.accountUserInfo, langId);
            pageHelper.registerInfoLink = langCode + "/" + mainAccountPage.url + "/" + newsletterPage.url + ".html";


            // Newsletter
            var newsletterItem = db.tbl_newsletterUser.Where(a => a.email == userItem.email).FirstOrDefault();

            if (newsletterItem != null)
            {
                pageHelper.isNewsletterRegister = true;
            }

            // Address 
            var addressList = ash.getAddressListByUserId(cartItem.userId);
            if (addressList != null && addressList.Count > 0)
            {
                pageHelper.lastAddressItem = addressList.LastOrDefault();
            }
            var addressPage = ps.getPageByType(pageType.accountAddress, langId);
            pageHelper.registerAddressLink = langCode + "/" + mainAccountPage.url + "/" + addressPage.url + ".html";

            //Order 
            var pageOrder = ps.getPageByType(pageType.accountOrders, langId);
            pageHelper.registerOrderLink = langCode + "/" + mainAccountPage.url + "/" + pageOrder.url + ".html";
            pageHelper.lastOrder = null; // ToDo: After The order

            return View(pageHelper);
        }

        [userTypeControl(userType.normalMember, userType.facebookMember)]
        [cartSummaryBind]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Dashboard(int pageId, string newsletterChange)
        {
            topCart cartItem = (topCart)ViewData["topCart"];
            userShared us = new userShared(db);

            var userItem = us.getUserById(cartItem.userId);
            var newsletterItem = db.tbl_newsletterUser.Where(a => a.email == userItem.email).FirstOrDefault();

            // Add Newsletter
            if (newsletterItem == null)
            {
                tbl_newsletterUser newsletterNewItem = new tbl_newsletterUser();

                newsletterNewItem.createTime = DateTime.Now;
                newsletterNewItem.email = userItem.email;
                newsletterNewItem.ipNo = getUserIP();

                try
                {
                    db.tbl_newsletterUser.Add(newsletterNewItem);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    errorSend(ex, "Newsletter Add", true);
                }
            }
            else // Remove Newsletter
            {
                try
                {
                    db.tbl_newsletterUser.Remove(newsletterItem);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    errorSend(ex, "Newsletter Remove", true);
                }

            }

            return Dashboard(pageId);

        }

        // Account Link  => Redirect Dashboard
        public ActionResult Main(int pageId)
        {
            pageShared ps = new pageShared(db);

            var pageItem = ps.getPageById(pageId);
            var dashboardPage = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.accountDashboard).FirstOrDefault();

            if (pageItem != null && dashboardPage != null)
            {
                return Redirect("~/" + langCode + "/" + pageItem.url + "/" + dashboardPage.url + ".html");
            }

            return null;
        }

        #endregion

        #region userInfo

        [cartSummaryBind]
        [titleDescriptionBinder]
        [userTypeControl(userType.normalMember, userType.facebookMember)]
        public ActionResult UserInfo(int pageId)
        {
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);
            addressShared ash = new addressShared(db);

            var pageItem = ps.getPageById(pageId);
            var mainAccountPage = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();


            topCart cartItem = (topCart)ViewData["topCart"];

            helperUserInfo pageHelper = new helperUserInfo();

            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.setTitle(pageItem.name);
            pageHelper.detail = pageItem.detail;

            pageHelper.leftMenuList = generateLeftMenu(mainAccountPage, pageItem.url);
            pageHelper.breadCrumbItem = getBreadCrumbTwoPage(mainAccountPage.name, mainAccountPage.url, pageItem.name, pageItem.url);


            var userItem = us.getUserById(cartItem.userId);

            pageHelper.name = userItem.name;
            pageHelper.surname = userItem.surname;
            pageHelper.email = userItem.email;

            if (userItem.birthday.HasValue)
            {
                var birthday = userItem.birthday.Value;

                pageHelper.day = birthday.Day;
                pageHelper.month = birthday.Month;
                pageHelper.year = birthday.Year;
            }

            if (userItem.gender.HasValue)
            {
                pageHelper.gender = userItem.gender.Value;
            }

            pageHelper.cancelUrl = langCode + "/" + mainAccountPage.url + ".html";

            return View(pageHelper);
        }


        [cartSummaryBind]
        [titleDescriptionBinder]
        [userTypeControl(userType.normalMember, userType.facebookMember)]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UserInfo(int pageId, helperUserInfo pageHelper)
        {
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);
            addressShared ash = new addressShared(db);

            var pageItem = ps.getPageById(pageId);
            var mainAccountPage = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();

            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.setTitle(pageItem.name);
            pageHelper.detail = pageItem.detail;

            pageHelper.leftMenuList = generateLeftMenu(mainAccountPage, pageItem.url);
            pageHelper.breadCrumbItem = getBreadCrumbTwoPage(mainAccountPage.name, mainAccountPage.url, pageItem.name, pageItem.url);
            pageHelper.cancelUrl = langCode + "/" + mainAccountPage.url + ".html";

            DateTime birthday = DateTime.Now;

            try
            {
                birthday = new DateTime(pageHelper.year, pageHelper.month, pageHelper.day);
            }
            catch
            {
                ModelState.AddModelError("validDate", lang.formValidDate);
            }

            try
            {
                System.Net.Mail.MailAddress mailItem = new System.Net.Mail.MailAddress(pageHelper.email);
            }
            catch
            {
                ModelState.AddModelError("email", lang.formValidEmail);
            }

            if (ModelState.IsValid)
            {
                topCart cartItem = (topCart)ViewData["topCart"];
                var userItem = us.getUserById(cartItem.userId);

                try
                {
                    userItem.birthday = birthday;
                    userItem.email = pageHelper.email;
                    userItem.gender = pageHelper.gender;
                    userItem.name = pageHelper.name;
                    userItem.surname = pageHelper.surname;
                    db.SaveChanges();

                    pageHelper.message = getSuccesMessage(lang.userInfoUpdated, "autoHide");
                    pageHelper.isMessageExist = true;
                }
                catch (Exception ex)
                {
                    errorSend(ex, "userAccountUpdate", true);
                }

            }
            else
            {
                string messages = string.Join("<br/> ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));


                pageHelper.message = getErrorMessage(messages, "autoHide");
                pageHelper.isMessageExist = true;
            }



            return View(pageHelper);
        }


        #endregion

        #region ChangePassword

        [cartSummaryBind]
        [titleDescriptionBinder]
        [userTypeControl(userType.normalMember, userType.facebookMember)]
        public ActionResult ChangePassword(int pageId)
        {
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);
            addressShared ash = new addressShared(db);

            var pageItem = ps.getPageById(pageId);
            var mainAccountPage = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();

            helperChangePassword pageHelper = new helperChangePassword();

            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.setTitle(pageItem.name);
            pageHelper.detail = pageItem.detail;

            pageHelper.leftMenuList = generateLeftMenu(mainAccountPage, pageItem.url);
            pageHelper.breadCrumbItem = getBreadCrumbTwoPage(mainAccountPage.name, mainAccountPage.url, pageItem.name, pageItem.url);
            pageHelper.cancelUrl = langCode + "/" + mainAccountPage.url + ".html";

            return View(pageHelper);
        }

        [cartSummaryBind]
        [titleDescriptionBinder]
        [userTypeControl(userType.normalMember, userType.facebookMember)]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(int pageId, helperChangePassword pageHelper)
        {
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);
            addressShared ash = new addressShared(db);

            var pageItem = ps.getPageById(pageId);
            var mainAccountPage = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();

            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.setTitle(pageItem.name);
            pageHelper.detail = pageItem.detail;
            pageHelper.leftMenuList = generateLeftMenu(mainAccountPage, pageItem.url);
            pageHelper.breadCrumbItem = getBreadCrumbTwoPage(mainAccountPage.name, mainAccountPage.url, pageItem.name, pageItem.url);
            pageHelper.cancelUrl = langCode + "/" + mainAccountPage.url + ".html";


            if (pageHelper.password != pageHelper.passwordRep)
            {
                ModelState.AddModelError("passwordRep", lang.formPassworRepSame);
            }

            if (ModelState.IsValid)
            {
                topCart cartItem = (topCart)ViewData["topCart"];

                try
                {
                    us.updateUserPassword(cartItem.userId, MD5(pageHelper.password));
                    pageHelper.message = getSuccesMessage(lang.updatePasswordSuccess, "autoHide");
                    pageHelper.isMessageExist = true;

                }
                catch (Exception ex)
                {
                    errorSend(ex, "Account Password Update", true);
                }
            }
            else
            {
                string messages = string.Join("<br /> ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));


                pageHelper.message = getErrorMessage(messages, "autoHide");
                pageHelper.isMessageExist = true;
            }



            return View(pageHelper);
        }


        #endregion

        #region Address

        #region Index

        [cartSummaryBind]
        [titleDescriptionBinder]
        [userTypeControl(userType.normalMember, userType.facebookMember)]
        public ActionResult AddressIndex(int pageId)
        {
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);
            addressShared ash = new addressShared(db);

            var pageItem = ps.getPageById(pageId);
            var mainAccountPage = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();

            helperAddress pageHelper = new helperAddress();

            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.setTitle(pageItem.name);
            pageHelper.detail = pageItem.detail;

            pageHelper.leftMenuList = generateLeftMenu(mainAccountPage, pageItem.url);
            pageHelper.breadCrumbItem = getBreadCrumbTwoPage(mainAccountPage.name, mainAccountPage.url, pageItem.name, pageItem.url);

            topCart cartItem = (topCart)ViewData["topCart"];
            var userItem = us.getUserById(cartItem.userId);

            pageHelper.userguid = userItem.guid;
            pageHelper.addressList = ash.getAddressListTemplate(userItem.userId);


            return View(pageHelper);
        }

        #endregion

        #region Add

        [userTypeControl(userType.normalMember, userType.facebookMember)]
        public JsonResult AddressAdd(string userGuid)
        {
            addressModelItem item = new addressModelItem();
            userShared us = new userShared(db);

            var userItem = us.getUserByGuid(userGuid);

            //Close Url
            pageShared ps = new pageShared(db);
            var mainPage = ps.getPageByType(pageType.account, langId);
            var addressPage = ps.getPageByType(pageType.accountAddress, langId);
            item.closeUrl = getSiteName(Request) + langCode + "/" + mainPage.url + "/" + addressPage.url + ".html";

            if (userItem == null)
            {
                item.isMessageExist = true;
                item.message = getErrorMessage(lang.unexpectedErrorMsg);

                return Json(new { html = RenderRazorViewToString("AddressAdd", item) }, JsonRequestBehavior.AllowGet);
            }

            item.guid = userItem.guid;
            item.addressItem = new tbl_address();
            item.addressItem.isPersonal = true;

            string htmlText = RenderRazorViewToString("AddressAdd", item);
            return Json(new { html = htmlText }, JsonRequestBehavior.AllowGet);
        }

        [userTypeControl(userType.normalMember, userType.facebookMember)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddressAdd(addressModelItem item)
        {
            System.Threading.Thread.Sleep(1500);

            //Close Url
            pageShared ps = new pageShared(db);
            addressShared ads = new addressShared(db);
            var mainPage = ps.getPageByType(pageType.account, langId);
            var addressPage = ps.getPageByType(pageType.accountAddress, langId);
            item.closeUrl = getSiteName(Request) + langCode + "/" + mainPage.url + "/" + addressPage.url + ".html";
            int addressId = 0;

            userShared us = new userShared(db);
            var userItem = us.getUserByGuid(item.guid);

            if (userItem == null)
            {
                item.isMessageExist = true;
                item.message = getErrorMessage(lang.unexpectedErrorMsg);

                return Json(new { html = RenderRazorViewToString("AddressAdd", item) }, JsonRequestBehavior.AllowGet);
            }

            extraValidation(item);

            if (ModelState.IsValid)
            {

                try
                {
                    item.addressItem.userId = userItem.userId;
                    item.addressItem.statu = true;
                    item.addressItem = ads.addAddress(item.addressItem);

                    item.isMessageExist = true;
                    item.message = getSuccesMessage(lang.addressAddSuccess);
                    item.isSuccess = true;
                    addressId = item.addressItem.addressId;

                }
                catch (Exception ex)
                {
                    errorSend(ex, "Account Address Add", true);

                    item.isMessageExist = true;
                    item.message = getErrorMessage(lang.unexpectedErrorMsg);

                }


            }


            string htmlText = RenderRazorViewToString("AddressAdd", item);
            return Json(new { html = htmlText, addressId = addressId });
        }

        #endregion

        #region Detail

        [userTypeControl(userType.normalMember, userType.facebookMember)]
        public JsonResult AddressDetail(string userGuid, int addressId)
        {
            userShared us = new userShared(db);
            addressShared ads = new addressShared(db);

            var userItem = us.getUserByGuid(userGuid);
            var addressItem = ads.getAddressById(addressId);

            addressModelItem item = new addressModelItem();

            if (userItem == null || addressItem == null || addressItem.userId != userItem.userId)
            {
                item.isMessageExist = true;
                item.message = getErrorMessage(lang.unexpectedErrorMsg);
            }

            item.addressItem = addressItem;

            return Json(new { html = RenderRazorViewToString("AddressDetail", item) }, JsonRequestBehavior.AllowGet);


        }

        #endregion

        #region Edit

        [userTypeControl(userType.normalMember, userType.facebookMember)]
        public JsonResult AddressEdit(string userGuid, int addressId)
        {
            userShared us = new userShared(db);
            addressShared ads = new addressShared(db);

            var userItem = us.getUserByGuid(userGuid);
            var addressItem = ads.getAddressById(addressId);

            addressModelItem item = new addressModelItem();

            if (userItem == null || addressItem == null || addressItem.userId != userItem.userId)
            {
                item.isMessageExist = true;
                item.message = getErrorMessage(lang.unexpectedErrorMsg);
            }

            item.addressItem = addressItem;
            item.guid = userGuid;

            //Close Url
            pageShared ps = new pageShared(db);
            var mainPage = ps.getPageByType(pageType.account, langId);
            var addressPage = ps.getPageByType(pageType.accountAddress, langId);
            item.closeUrl = getSiteName(Request) + langCode + "/" + mainPage.url + "/" + addressPage.url + ".html";



            string htmlText = RenderRazorViewToString("AddressEdit", item);
            return Json(new { html = htmlText }, JsonRequestBehavior.AllowGet);

        }


        [userTypeControl(userType.normalMember, userType.facebookMember)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddressEdit(addressModelItem item)
        {
            System.Threading.Thread.Sleep(1500);

            //Close Url
            pageShared ps = new pageShared(db);
            addressShared ads = new addressShared(db);
            var mainPage = ps.getPageByType(pageType.account, langId);
            var addressPage = ps.getPageByType(pageType.accountAddress, langId);
            item.closeUrl = getSiteName(Request) + langCode + "/" + mainPage.url + "/" + addressPage.url + ".html";

            userShared us = new userShared(db);
            var userItem = us.getUserByGuid(item.guid);

            if (userItem == null || item.addressItem == null || item.addressItem.userId != userItem.userId)
            {
                item.isMessageExist = true;
                item.message = getErrorMessage(lang.unexpectedErrorMsg, "autoHide");

                return Json(new { html = RenderRazorViewToString("AddressEdit", item) }, JsonRequestBehavior.AllowGet);
            }

            extraValidation(item);

            if (ModelState.IsValid)
            {
                try
                {
                    item.addressItem.userId = userItem.userId;
                    item.addressItem.statu = true;

                    var updateItem = ads.getAddressById(item.addressItem.addressId);
                    db.Entry<tbl_address>(updateItem).CurrentValues.SetValues(item.addressItem);
                    db.SaveChanges();

                    item.isMessageExist = true;
                    item.message = getSuccesMessage(lang.addressEditSuccess, "autoHide");
                    item.isSuccess = true;

                }
                catch (Exception ex)
                {
                    errorSend(ex, "Account Address Edit", true);
                    item.isMessageExist = true;
                    item.message = getErrorMessage(lang.unexpectedErrorMsg, "autoHide");
                }
            }

            string htmlText = RenderRazorViewToString("AddressEdit", item);
            return Json(new { html = htmlText });

        }


        #endregion

        #region Delete

        [userTypeControl(userType.normalMember, userType.facebookMember)]
        public JsonResult AddressDelete(string userGuid, int addressId)
        {
            userShared us = new userShared(db);
            addressShared ads = new addressShared(db);

            var userItem = us.getUserByGuid(userGuid);
            var addressItem = ads.getAddressById(addressId);

            addressModelItem item = new addressModelItem();

            if (userItem == null || addressItem == null || addressItem.userId != userItem.userId)
            {
                item.isMessageExist = true;
                item.message = getErrorMessage(lang.unexpectedErrorMsg);
                item.addressItem = new tbl_address();
                item.guid = "";
            }
            else
            {
                item.guid = userItem.guid;
                item.addressItem = addressItem;
            }

            //Close Url
            pageShared ps = new pageShared(db);
            var mainPage = ps.getPageByType(pageType.account, langId);
            var addressPage = ps.getPageByType(pageType.accountAddress, langId);
            item.closeUrl = getSiteName(Request) + langCode + "/" + mainPage.url + "/" + addressPage.url + ".html";



            return Json(new { html = RenderRazorViewToString("AddressDelete", item) }, JsonRequestBehavior.AllowGet);

        }

        [userTypeControl(userType.normalMember, userType.facebookMember)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddressDelete(addressModelItem item)
        {
            System.Threading.Thread.Sleep(1500);

            //Close Url
            pageShared ps = new pageShared(db);
            addressShared ads = new addressShared(db);
            var mainPage = ps.getPageByType(pageType.account, langId);
            var addressPage = ps.getPageByType(pageType.accountAddress, langId);
            item.closeUrl = getSiteName(Request) + langCode + "/" + mainPage.url + "/" + addressPage.url + ".html";

            userShared us = new userShared(db);
            var userItem = us.getUserByGuid(item.guid);

            if (userItem == null || item.addressItem == null || item.addressItem.userId != userItem.userId)
            {
                item.isMessageExist = true;
                item.message = getErrorMessage(lang.unexpectedErrorMsg, "autoHide");

                return Json(new { html = RenderRazorViewToString("AddressEdit", item) }, JsonRequestBehavior.AllowGet);
            }

            try
            {

                ads.updateAddressIsDeleted(item.addressItem.addressId);
                item.isMessageExist = true;
                item.message = getSuccesMessage(lang.deleteAddressSuccess);
                item.isSuccess = true;

            }
            catch (Exception ex)
            {
                errorSend(ex, "Account Address Delete", true);
                item.isMessageExist = true;
                item.message = getErrorMessage(lang.unexpectedErrorMsg, "autoHide");
            }

            string htmlText = RenderRazorViewToString("AddressDelete", item);
            return Json(new { html = htmlText });

        }


        #endregion

        private void extraValidation(addressModelItem item)
        {

            if (item.addressItem.isPersonal)
            {
                string tcPattern = @"^\d{11}$";

                if (string.IsNullOrWhiteSpace(item.addressItem.tcNo) || !Regex.IsMatch(item.addressItem.tcNo, tcPattern))
                {
                    ModelState.AddModelError("addressItem.tcNo", lang.addressTcNoRequired);
                }
            }
            else
            {
                string taxNoPattern = @"^\d{10}$";

                if (string.IsNullOrWhiteSpace(item.addressItem.taxNo) || !Regex.IsMatch(item.addressItem.taxNo, taxNoPattern))
                {
                    ModelState.AddModelError("addressItem.taxNo", lang.addressTaxNoRequired);
                }

                if (string.IsNullOrWhiteSpace(item.addressItem.taxOffice))
                {
                    ModelState.AddModelError("addressItem.taxOffice", lang.addressTaxOfficeRequired);
                }

                if (string.IsNullOrWhiteSpace(item.addressItem.companyName))
                {
                    ModelState.AddModelError("addressItem.taxOffice", lang.addressCompanyNameRequired);
                }
            }

        }

        #endregion

        #region Order

        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult OrderIndex(int pageId)
        {
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);
            topCart cartItem = (topCart)ViewData["topCart"];

            var pageMainAccount = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();

            var orderSearchPage = ps.getPageByType(pageType.accountOrderSearch, langId);
            var orderDetailPage = ps.getPageByType(pageType.accountOrderDetail, langId);

            if (!cartItem.isRegisteredUser)
            {
                return Redirect("~/" + langCode + "/" + orderSearchPage.url + ".html");
            }

            var pageItem = ps.getPageById(pageId);


            helperOrder pageHelper = new helperOrder();

            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.setTitle(pageItem.name);
            pageHelper.detail = pageItem.detail;
            pageHelper.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);
            pageHelper.leftMenuList = generateLeftMenu(pageMainAccount, pageItem.url);



            string orderDatailLinkPrefix = langCode + "/" + orderDetailPage.url + ".html?orderGuid=";
            pageHelper.orderList = getOrderList(orderDatailLinkPrefix, cartItem.userId, true);

            return View(pageHelper);
        }

        private List<ViewModel.Account.Order.orderItem> getOrderList(string orderDatailLinkPrefix, int userId, bool isRegisteredUser)
        {
            orderShared os = new orderShared(db);
            List<ViewModel.Account.Order.orderItem> helper = new List<ViewModel.Account.Order.orderItem>();

            var orderList = os.getOrderList(userId, isRegisteredUser);

            var moneyCulture = CultureInfo.CreateSpecificCulture("en-US");

            foreach (var item in orderList)
            {
                ViewModel.Account.Order.orderItem helperItem = new ViewModel.Account.Order.orderItem();

                helperItem.orderDateStr = item.createDate.ToString("dd.mm.yyyy");
                helperItem.orderDetailLink = orderDatailLinkPrefix + item.orderGuid;
                helperItem.orderNo = item.orderNo;
                helperItem.orderStatu = os.getOrderStatuString(item.orderStatu);
                helperItem.totalPriceStr = item.totalCheckoutPrice.ToString("F2", moneyCulture);

                helper.Add(helperItem);
            }


            return helper;
        }


        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult OrderSearch(int pageId)
        {
            topCart cartItem = (topCart)ViewData["topCart"];
            pageShared ps = new pageShared(db);
            var pageMainAccount = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();



            if (cartItem.isRegisteredUser)
            {

                var pageOrderList = ps.getPageByType(pageType.accountOrders, langId);
                return Redirect("~/" + langCode + "/" + pageMainAccount.url + "/" + pageOrderList.url + ".html");
            }


            var pageItem = db.tbl_page.Include("tbl_category").Where(a => a.pageId == pageId).FirstOrDefault();



            helperOrderSearch pageHelper = new helperOrderSearch();

            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.detail = pageItem.detail;
            pageHelper.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);

            var parentCategoryList = db.tbl_category.Include("tbl_page").Where(a => a.parentId == pageItem.tbl_category.parentId && a.statu).ToList();
            pageHelper.leftMenuList = getStaticLeftMenu(parentCategoryList, pageId);

            return View(pageHelper);


        }

        [cartSummaryBind]
        [titleDescriptionBinder]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderSearch(helperOrderSearch pageHelper, int pageId)
        {
            topCart cartItem = (topCart)ViewData["topCart"];
            pageShared ps = new pageShared(db);

            var pageMainAccount = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();

            if (cartItem.isRegisteredUser)
            {
                var pageOrderList = ps.getPageByType(pageType.accountOrders, langId);
                return Redirect("~/" + langCode + "/" + pageMainAccount.url + "/" + pageOrderList.url + ".html");
            }

            var pageItem = db.tbl_page.Include("tbl_category").Where(a => a.pageId == pageId).FirstOrDefault();
            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.detail = pageItem.detail;
            pageHelper.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);

            var parentCategoryList = db.tbl_category.Include("tbl_page").Where(a => a.parentId == pageItem.tbl_category.parentId && a.statu).ToList();
            pageHelper.leftMenuList = getStaticLeftMenu(parentCategoryList, pageId);

            // Redirect to Login Page With Session
            if (!string.IsNullOrWhiteSpace(pageHelper.toLogin))
            {
                Session["redirectPage"] = Request.Url.ToString();

                var loginPage = ps.getPageByType(pageType.registerLogin, langId);
                if (loginPage == null)
                {
                    errorSend(new Exception("loginPage gelmemesi, checkout Step 0"), "LoginPage gelmemesi, checkout Step 0", true);
                    return null;
                }

                return Redirect("~/" + langCode + "/" + loginPage.url + ".html");
            }
            else // Search Order
            {
                if (ModelState.IsValid)
                {

                    orderShared os = new orderShared(db);
                    var orderItem = os.getUnregisteredOrder(pageHelper.email, pageHelper.orderNo);

                    if (orderItem != null)
                    {
                        var orderDetailPage = ps.getPageByType(pageType.accountOrderDetail, langId);
                        return Redirect("~/" + langCode + "/" + orderDetailPage.url + ".html?orderGuid=" + orderItem.orderGuid);
                    }
                    else
                    {
                        pageHelper.isMessageExist = true;
                        pageHelper.message = getErrorMessage(lang.orderSearchNotFound);
                        return View(pageHelper);
                    }

                }
                else
                {
                    pageHelper.isMessageExist = true;
                    pageHelper.message = getErrorMessage(getModelStateError(ModelState));
                    return View(pageHelper);
                }
            }


        }


        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult OrderDetail(int pageId, string orderGuid)
        {
            orderShared os = new orderShared(db);
            addressShared ads = new addressShared(db);

            if (!string.IsNullOrWhiteSpace(orderGuid))
            {

                var orderItem = os.getOrderByGuid(orderGuid);

                if (orderItem != null)
                {
                    pageShared ps = new pageShared(db);
                    var pageItem = ps.getPageById(pageId);

                    var orderPage = ps.getPageByType(pageType.accountOrders, langId);


                    helperOrderDetail pageHelper = new helperOrderDetail();

                    ps.pageTitleBind(pageItem, pageHelper, langId);
                    pageHelper.setTitle(pageItem.name);
                    pageHelper.detail = pageItem.detail;


                    pageHelper.orderSummary = os.getOrderSummary(orderItem);

                    pageHelper.orderNo = orderItem.orderNo;
                    pageHelper.orderStatuHtml = os.getOrderStatuString(orderItem.orderStatu);

                    // On Cargo Add Track Url 
                    if ((orderStatu)orderItem.orderStatu == orderStatu.onCargo && !string.IsNullOrWhiteSpace(orderItem.shipmentNo))
                    {
                        pageHelper.orderStatuHtml = pageHelper.orderStatuHtml + " " + os.getCargoTrackHtml(orderItem);
                    }

                    pageHelper.salesAgreement = orderItem.salesAgreement;
                    pageHelper.preSalesAgreement = orderItem.preSalesAgreement;

                    var deliveryAddressItem = ads.getAddressHtmlFromObj(orderItem.deliveryAddressId, orderItem.deliveryAddressObj);
                    var billingAddressItem = ads.getAddressHtmlFromObj(orderItem.billingAddressId, orderItem.billingAddressObj);

                    pageHelper.deliveryAddress = ads.getAddressHtml(deliveryAddressItem, AddressHtmlType.orderDetail, this);
                    pageHelper.billingAddress = ads.getAddressHtml(billingAddressItem, AddressHtmlType.orderDetail, this);

                    pageHelper.orderNote = orderItem.orderNote;

                    topCart cartItem = (topCart)ViewData["topCart"];

                    if (cartItem.isRegisteredUser)
                    {
                        pageHelper.isGoBackExist = true;

                        var mainAccountPage = ps.getPageByType(pageType.account, langId);
                        var orderListPage = ps.getPageByType(pageType.accountOrders, langId);

                        pageHelper.goBackLink = langCode + "/" + mainAccountPage.url + "/" + orderListPage.url + ".html";


                    }

                    return View(pageHelper);

                }
            }


            return null;

        }

        #endregion

        #region Discount

        [cartSummaryBind]
        [titleDescriptionBinder]
        [userTypeControl(userType.normalMember, userType.facebookMember)]
        public ActionResult DiscountIndex(int pageId)
        {
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);
            addressShared ash = new addressShared(db);

            var pageItem = ps.getPageById(pageId);
            var mainAccountPage = db.tbl_page.Include("tbl_category").Where(a => a.pageTypeId == (int)pageType.account).FirstOrDefault();

            helperDiscount pageHelper = new helperDiscount();

            ps.pageTitleBind(pageItem, pageHelper, langId);
            pageHelper.setTitle(pageItem.name);
            pageHelper.detail = pageItem.detail;

            pageHelper.leftMenuList = generateLeftMenu(mainAccountPage, pageItem.url);
            pageHelper.breadCrumbItem = getBreadCrumbTwoPage(mainAccountPage.name, mainAccountPage.url, pageItem.name, pageItem.url);


            return View(pageHelper);
        }

        #endregion

        #region Helper

        private breadCrumb getBreadCrumbStaticPage(string pageName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = pageName;
            helperItem.url = "#";

            return helperItem;
        }

        private breadCrumb getBreadCrumbTwoPage(string pageName1, string pageUrl1, string pageName2, string pageUrl2)
        {
            breadCrumb item = new breadCrumb();

            item.url = Url.Content("~/") + langCode + "/" + pageUrl1 + ".html";
            item.name = pageName1;

            item.child = new breadCrumb();

            item.child.url = Url.Content("~/") + langCode + "/" + pageUrl1 + "/" + pageUrl2 + ".html";
            item.child.name = pageName2;


            return item;

        }

        public List<leftMenuItem> generateLeftMenu(tbl_page mainAccountPage, string currentPageUrl)
        {
            List<leftMenuItem> menuList = new List<leftMenuItem>();

            if (mainAccountPage != null && mainAccountPage.tbl_category != null)
            {

                var parentCategory = mainAccountPage.tbl_category;
                var subCategoryList = db.tbl_category.Include("tbl_page").Where(a => a.parentId == parentCategory.categoryId && a.tbl_page.Count > 0).ToList();

                foreach (var item in subCategoryList)
                {
                    leftMenuItem menuItem = new leftMenuItem();

                    menuItem.url = langCode + "/" + mainAccountPage.url + "/" + item.tbl_page.FirstOrDefault().url + ".html";
                    menuItem.name = item.name;

                    if (item.tbl_page.FirstOrDefault().url == currentPageUrl)
                    {
                        menuItem.className = "active";
                    }

                    menuList.Add(menuItem);
                }
            }

            return menuList;

        }

        private List<leftMenuItem> getStaticLeftMenu(List<tbl_category> list, int selectedPageId)
        {
            List<leftMenuItem> helperList = new List<leftMenuItem>();

            pageShared ps = new pageShared(db);

            foreach (var item in list.Where(a => a.statu).OrderBy(a => a.sequence))
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

        private string getUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }



        #endregion

    }

}
