using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HelperSite.DbController;
using HelperSite.Shared;

namespace HelperSite.Routing
{
    public class CustomRouting : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData result = null;

            var rootList = getCustomPath(httpContext);
            if (rootList != null && rootList.Count > 0)
            {
                result = new RouteData(this, new MvcRouteHandler());
                result.DataTokens.Add("namespaces", new string[] { "titizOto.Controllers" });
                foreach (var item in rootList)
                {
                    result.Values.Add(item.Key, item.Value);
                }
            }

            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }

        public Dictionary<string, string> getCustomPath(HttpContextBase httpContext)
        {
            List<string> linkList = httpContext.Request.AppRelativeCurrentExecutionFilePath.Replace(".html", string.Empty).Split('/').Where(a => a != "~").ToList();
            Dictionary<string, string> returnList = new Dictionary<string, string>();

            int langId = 1;

            DbWithBasicFunction dbc = new DbWithBasicFunction();
            var db = dbc.db;

            #region 2 link

            if (linkList.Count == 2)
            {
                string lang = linkList[0];
                string c1Url = linkList[1];

                if (lang == "en")
                {
                    langId = 2;
                }

                var pageItem = db.tbl_page.Where(a => a.url == c1Url && a.statu && a.langId == langId).FirstOrDefault();

                if (pageItem == null)
                {
                    return returnList;
                }

                switch (pageItem.pageTypeId)
                {
                    case (int)pageType.allBrand:

                        // /tr/modeller.html
                        returnList.Add("lang", lang);
                        returnList.Add("controller", "AllBrand");
                        returnList.Add("action", "Index");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;

                    case (int)pageType.productList:

                        // /tr/krom-aksesuar.html
                        returnList.Add("lang", lang);
                        returnList.Add("controller", "ProductList");
                        returnList.Add("action", "Index");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;

                    case (int)pageType.normalIcerik:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "Page");
                        returnList.Add("action", "Index");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;

                    case (int)pageType.openContent:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "Page");
                        returnList.Add("action", "OpenContent");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;

                    case (int)pageType.registerLogin:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "LoginRegister");
                        returnList.Add("action", "Index");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;

                    case (int)pageType.activationResent:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "LoginRegister");
                        returnList.Add("action", "ActivationResent");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;

                    case (int)pageType.basket:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "Basket");
                        returnList.Add("action", "Index");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;

                    case (int)pageType.forgetPassword:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "LoginRegister");
                        returnList.Add("action", "ForgetPassword");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;


                    case (int)pageType.account:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "Account");
                        returnList.Add("action", "Main");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;


                    case (int)pageType.search:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "Search");
                        returnList.Add("action", "Index");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;


                    case (int)pageType.checkoutMain:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "Checkout");
                        returnList.Add("action", "Main");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        break;

                    case (int)pageType.accountOrderSearch:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "Account");
                        returnList.Add("action", "OrderSearch");
                        returnList.Add("pageId", pageItem.pageId.ToString());

                        break;


                    case (int)pageType.accountOrderDetail:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "Account");
                        returnList.Add("action", "OrderDetail");
                        returnList.Add("pageId", pageItem.pageId.ToString());

                        break;

                }

            }

            #endregion

            #region 3 link

            if (linkList.Count == 3)
            {
                string lang = linkList[0];
                string c1Url = linkList[1];
                string c2Url = linkList[2];

                if (lang == "en")
                {
                    langId = 2;
                }

                var pageItem = db.tbl_page.Where(a => a.url == c1Url && a.statu && a.langId == langId).FirstOrDefault();

                if (pageItem == null)
                {
                    return null;
                }

                switch (pageItem.pageTypeId)
                {
                    case (int)pageType.productList:

                        // /tr/krom-aksesuar/bmw.html 
                        var brandItem = db.tbl_carBrand.Where(a => a.statu && a.url == c2Url && a.langId == langId).FirstOrDefault();

                        if (brandItem != null)
                        {
                            returnList.Add("lang", lang);
                            returnList.Add("carBrandId", brandItem.carBrandId.ToString());
                            returnList.Add("parentUrl", pageItem.url.ToString());
                            returnList.Add("parentName", pageItem.name);
                            returnList.Add("controller", "ProductList");
                            returnList.Add("action", "BrandSelect");
                        }

                        break;

                    case (int)pageType.activation:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "LoginRegister");
                        returnList.Add("action", "ActivationUpdate");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        returnList.Add("activationCode", c2Url);

                        break;


                    case (int)pageType.resetPassword:

                        returnList.Add("lang", lang);
                        returnList.Add("controller", "LoginRegister");
                        returnList.Add("action", "ResetPassword");
                        returnList.Add("pageId", pageItem.pageId.ToString());
                        returnList.Add("resetCode", c2Url);
                        break;


                    case (int)pageType.checkoutMain:

                        var subCheckoutPageItem = db.tbl_page.Where(a => a.url == c2Url && a.statu && a.langId == langId).FirstOrDefault();

                        #region Sub Checkout Page

                        if (subCheckoutPageItem != null)
                        {
                            switch (subCheckoutPageItem.pageTypeId)
                            {
                                case (int)pageType.checkoutRegisterStatu:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Checkout");
                                    returnList.Add("action", "CheckoutOption");
                                    returnList.Add("pageId", subCheckoutPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.checkoutDelivery:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Checkout");
                                    returnList.Add("action", "Delivery");
                                    returnList.Add("pageId", subCheckoutPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.checkoutBilling:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Checkout");
                                    returnList.Add("action", "Billing");
                                    returnList.Add("pageId", subCheckoutPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.checkoutCargo:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Checkout");
                                    returnList.Add("action", "Cargo");
                                    returnList.Add("pageId", subCheckoutPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.checkoutPayment:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Checkout");
                                    returnList.Add("action", "PaymentOption");
                                    returnList.Add("pageId", subCheckoutPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.checkoutSummary:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Checkout");
                                    returnList.Add("action", "Summary");
                                    returnList.Add("pageId", subCheckoutPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.checkoutComplete:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Checkout");
                                    returnList.Add("action", "Complete");
                                    returnList.Add("pageId", subCheckoutPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.checkoutErrorProcess:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Checkout");
                                    returnList.Add("action", "ErrorProcess");
                                    returnList.Add("pageId", subCheckoutPageItem.pageId.ToString());

                                    break;
                            }
                        }

                        #endregion


                        break;


                    case (int)pageType.account:

                        var subPageItem = db.tbl_page.Where(a => a.url == c2Url && a.statu && a.langId == langId).FirstOrDefault();

                        #region Sub Acount Page

                        if (subPageItem != null)
                        {
                            switch (subPageItem.pageTypeId)
                            {

                                case (int)pageType.accountDashboard:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Account");
                                    returnList.Add("action", "Dashboard");
                                    returnList.Add("pageId", subPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.accountOrders:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Account");
                                    returnList.Add("action", "OrderIndex");
                                    returnList.Add("pageId", subPageItem.pageId.ToString());

                                    break;


                        

                                case (int)pageType.accountUserInfo:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Account");
                                    returnList.Add("action", "UserInfo");
                                    returnList.Add("pageId", subPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.accountPassword:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Account");
                                    returnList.Add("action", "ChangePassword");
                                    returnList.Add("pageId", subPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.accountAddress:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Account");
                                    returnList.Add("action", "AddressIndex");
                                    returnList.Add("pageId", subPageItem.pageId.ToString());

                                    break;

                                case (int)pageType.accountDiscount:

                                    returnList.Add("lang", lang);
                                    returnList.Add("controller", "Account");
                                    returnList.Add("action", "DiscountIndex");
                                    returnList.Add("pageId", subPageItem.pageId.ToString());

                                    break;
                            }
                        }

                        #endregion

                        break;
                }

            }

            #endregion

            #region 4 link

            if (linkList.Count == 4)
            {
                string lang = linkList[0];
                string c1Url = linkList[1];
                string c2Url = linkList[2];
                string c3Url = linkList[3];

                if (lang == "en")
                {
                    langId = 2;
                }

                var pageItem = db.tbl_page.Where(a => a.url == c1Url && a.statu && a.langId == langId).FirstOrDefault();

                if (pageItem == null)
                {
                    return returnList;
                }

                switch (pageItem.pageTypeId)
                {
                    case (int)pageType.productList:

                        // /tr/krom-aksesuar/bmw/x5.html 
                        var brandItem = db.tbl_carBrand.Where(a => a.statu && a.url == c2Url && a.langId == langId).FirstOrDefault();
                        var modelItem = db.tbl_carModel.Where(a => a.statu && a.url == c3Url && a.langId == langId).FirstOrDefault();

                        if (brandItem != null && modelItem != null)
                        {
                            returnList.Add("lang", lang);
                            returnList.Add("carBrandId", brandItem.carBrandId.ToString());
                            returnList.Add("parentUrl", pageItem.url.ToString());
                            returnList.Add("parentName", pageItem.name);
                            returnList.Add("carModelId", modelItem.carModelId.ToString());
                            returnList.Add("controller", "ProductList");
                            returnList.Add("action", "ModelSelect");
                        }

                        break;
                }
            }

            #endregion

            #region 5 link

            if (linkList.Count == 5)
            {
                string lang = linkList[0];
                string c1Url = linkList[1];
                string c2Url = linkList[2];
                string c3Url = linkList[3];
                string c4Url = linkList[4];

                if (lang == "en")
                {
                    langId = 2;
                }

                var pageItem = db.tbl_page.Where(a => a.url == c1Url && a.statu && a.langId == langId).FirstOrDefault();

                if (pageItem == null)
                {
                    return returnList;
                }

                switch (pageItem.pageTypeId)
                {
                    case (int)pageType.productList:

                        // /tr/krom-aksesuar/bmw/x5/arkaKoltuk.html 
                        var brandItem = db.tbl_carBrand.Where(a => a.statu && a.url == c2Url && a.langId == langId).FirstOrDefault();
                        var modelItem = db.tbl_carModel.Where(a => a.statu && a.url == c3Url && a.langId == langId).FirstOrDefault();
                        var productItem = db.tbl_product.Where(a => a.statu && a.isDeleted == false && a.url == c4Url && a.langId == langId).FirstOrDefault();

                        if (brandItem != null && modelItem != null && productItem != null)
                        {
                            returnList.Add("lang", lang);
                            returnList.Add("parentUrl", pageItem.url.ToString());
                            returnList.Add("parentName", pageItem.name);
                            returnList.Add("carBrandId", brandItem.carBrandId.ToString());
                            returnList.Add("carModelId", modelItem.carModelId.ToString());
                            returnList.Add("productId", productItem.productId.ToString());
                            returnList.Add("controller", "ProductDetail");
                            returnList.Add("action", "Detail");
                        }

                        break;
                }

            }

            #endregion

            return returnList;
        }
    }
}