using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelperSite.DbController;
using HelperSite.Shared;
using titizOto.Models;
using ViewModel.Shared;


namespace HelperSite.Attribute
{
    public class cartSummaryBind : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            DbWithController itemController = (DbWithController)filterContext.Controller;

            // check is Remember Me Cookie
            checkCookie(itemController.db, itemController.Session, filterContext.RequestContext.HttpContext.Request, itemController.Response, itemController);

            var cartItem = getTopCartItem(itemController.db, itemController.Session, filterContext.RequestContext.HttpContext.Request, itemController.Response, itemController.langId);

            itemController.ViewData["topCart"] = cartItem;



            base.OnActionExecuting(filterContext);
        }

        private topCart getTopCartItem(titizOtoEntities db, HttpSessionStateBase httpSessionStateBase, HttpRequestBase request, HttpResponseBase response, int langId)
        {
            topCart helperItem = new topCart();

            bool isGuest = true;

            if (httpSessionStateBase["userId"] != null)
            {
                helperItem = getUserCartItem(db, httpSessionStateBase, request, response, langId);

                if (helperItem != null)
                {
                    isGuest = false;
                }
            }


            if (isGuest)
            {
                helperItem = getGuestCartItem(db, httpSessionStateBase, request, response, langId);
            }

            bindBasketAndRegisterUrl(db, langId, helperItem);

            return helperItem;

        }

        private topCart getUserCartItem(titizOtoEntities db, HttpSessionStateBase httpSessionStateBase, HttpRequestBase request, HttpResponseBase response, int langId)
        {
            int userId = 0;
            topCart helperItem = new topCart();


            if (int.TryParse(httpSessionStateBase["userId"].ToString(), out userId))
            {

                var userItem = db.tbl_user.Where(a => a.userId == userId).FirstOrDefault();

                if (userItem != null)
                {
                    helperItem.isRegisteredUser = true;
                    helperItem.userId = userId;
                    helperItem.nameSurname = userItem.name + " " + userItem.surname;
                    helperItem.guestGuid = null;
                    helperItem.userGuid = userItem.guid;

                    var cartList = db.tbl_basket.Where(a => a.userId == userId).ToList();

                    if (cartList != null && cartList.Count > 0)
                    {
                        helperItem.basketIdString = string.Join(",", cartList.Select(a => a.basketId).ToList());
                        helperItem.productCount = cartList.Sum(a => a.quantity);
                    }
                    else
                    {
                        helperItem.productCount = 0;
                    }



                    

                    return helperItem;
                }


            }

            return null;


        }

        private topCart getGuestCartItem(titizOtoEntities db, HttpSessionStateBase httpSessionStateBase, HttpRequestBase request, HttpResponseBase response, int langId)
        {
            httpSessionStateBase["userId"] = null;

            topCart helperItem = new topCart();


            string guestGuid = "";
            if (httpSessionStateBase["guestGuid"] != null)
            {
                guestGuid = httpSessionStateBase["guestGuid"].ToString();
                if (guestGuid == "System.Web.HttpCookie" || guestGuid == "00000000-0000-0000-0000-000000000000")
                {
                    guestGuid = getGuidCookieOrNew(request, response);
                    httpSessionStateBase["guestGuid"] = guestGuid;
                }

            }
            else
            {
                guestGuid = getGuidCookieOrNew(request, response);
            }

            helperItem.guestGuid = guestGuid;

            var basketList = db.tbl_basket.Where(a => a.guestCode == guestGuid).ToList();
            if (basketList != null && basketList.Count > 0)
            {
                helperItem.basketIdString = string.Join(",", basketList.Select(a => a.basketId).ToList());
                helperItem.productCount = basketList.Sum(a => a.quantity);
            }


          

            return helperItem;
        }

        private string getGuidCookieOrNew(HttpRequestBase request, HttpResponseBase response)
        {
            if (request.Cookies["titizOto"] != null)
            {
                string guestGuid = request.Cookies["titizOto"].Value;
                if (guestGuid == "System.Web.HttpCookie" || guestGuid == "00000000-0000-0000-0000-000000000000")
                {
                    guestGuid = Guid.NewGuid().ToString();
                    response.Cookies["titizOto"].Expires = DateTime.Now.AddDays(-1);
                }

                return guestGuid;
            }
            else
            {
                string newGuestGuid = Guid.NewGuid().ToString();

                response.Cookies["titizOto"].Value = newGuestGuid;
                response.Cookies["titizOto"].Expires = DateTime.Now.AddMonths(3);
                return newGuestGuid;
            }
        }

        private void bindBasketAndRegisterUrl(titizOtoEntities db, int langId, topCart helperItem)
        {
            pageShared ps = new pageShared(db);

            var registerPage = ps.getPageByType(pageType.registerLogin, langId);
            if (registerPage != null)
            {
                helperItem.registerUrl = registerPage.url + ".html";
            }

            var basketPage = ps.getPageByType(pageType.basket, langId);
            if (basketPage != null)
            {
                helperItem.basketUrl = basketPage.url + ".html";
            }
        }

        private void checkCookie(titizOtoEntities db, HttpSessionStateBase httpSessionStateBase, HttpRequestBase request, HttpResponseBase response, DbWithController itemController)
        {

            if (httpSessionStateBase["userId"] == null && request.Cookies["userCookie"] != null && request.Cookies["userCookie"]["userHashVal"] != null && request.Cookies["userCookie"]["userHashValTwo"] != null)
            {

                var userList = db.tbl_user.Where(a => a.registerStatuId == (int)registerStatu.registered).ToList();

                tbl_user selectedUser = null;

                string userHashVal = request.Cookies["userCookie"]["userHashVal"];
                string userHashValTwo = request.Cookies["userCookie"]["userHashValTwo"];

                foreach (var item in userList)
                {
                    if (item.password.Length > 6 && userHashValTwo == item.password.Substring(0, 7) && itemController.MD5(item.email).Substring(0, 7) == userHashVal)
                    {
                        selectedUser = item;
                        break;

                    }
                }


                if (selectedUser != null)
                {
                    httpSessionStateBase["userId"] = selectedUser.userId.ToString();
                    httpSessionStateBase["userRoleId"] = selectedUser.userTypeId.ToString();
                }
                else
                {
                    response.Cookies["userCookie"].Expires = DateTime.Now.AddDays(-1);

                }

            }


        }

    }
}