using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using HelperSite.Attribute;
using HelperSite.DbController;
using HelperSite.Shared;
using ViewModel.Search;
using ViewModel.Shared;

namespace titizOto.Controllers
{
    public class SearchController : DbWithControllerWithMaster
    {
        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult Index(int pageId)
        {
            if (Request.QueryString["keyWord"] == null)
            {
                return null;
            }

            string searchWord = Request.QueryString["keyWord"];

            helperSearch helperItem = new helperSearch();
            pageShared ps = new pageShared(db);
            productShared pc = new productShared(db);

            var searchPage = ps.getPageByType(pageType.search, langId);
            helperItem.searchLink = Url.Content("~/") + langCode + "/" + searchPage.url + ".html";

            var item = db.tbl_page.Include("tbl_category").Where(a => a.pageId == pageId).FirstOrDefault();

            if (item == null)
            {
                return null;
            }


            helperItem.breadCrumbItem = getBreadCrumbStaticPage(item.name);
            helperItem.setTitle(item.name);
            helperItem.detail = item.detail;

            //Title
            ps.pageTitleBind(item, helperItem, langId);

            //string pattern = getPatternBySearchWord(searchWord);
            helperItem.searchWord = searchWord;

            //helperItem.searchList = new List<searchResultItem>();
            helperItem.searchList = getSearchResultList(searchWord);

            if (helperItem.searchList.Count > 0)
            {
                helperItem.resultFound = string.Format(App_GlobalResources.lang.totalResultFound, helperItem.searchList.Count);
            }
            else
            {
                helperItem.resultFound = App_GlobalResources.lang.noResultFound;
            }

            int productCount = helperItem.searchList.Count;
            int currentPage = pc.getCurrentPage(Request, productCount, 10);
            helperItem.pagingItem = pc.getPageItem(productCount, currentPage, 10, "?keyWord=" + searchWord + "&sayfa=");
            helperItem.searchList = helperItem.searchList.Skip((currentPage - 1) * 10).Take(10).ToList();
            helperItem.searchKeyWordList = Newtonsoft.Json.JsonConvert.SerializeObject(getSearchKeywordList(searchWord));

            return View(helperItem);
        }

        public List<searchResultItem> getSearchResultList(string searchWord)
        {
            List<searchResultItem> helperList = new List<searchResultItem>();

            productShared ps = new productShared(db);
            pageShared pas = new pageShared(db);

            //Regex regexItem = new Regex(pattern, RegexOptions.None);

            var currentCultureInfo = CultureInfo.CreateSpecificCulture(langCulture);
            var enCulture = CultureInfo.CreateSpecificCulture("en-US");


            string productUrl = "";
            var productPage = pas.getPageByType(pageType.productList, langId);

            if (productPage != null)
            {
                productUrl = productPage.url;
            }

            string searchText = "";

            #region Product

            // Search Product List
            var newProductList = db.tbl_product.Include("tbl_stock").Include("tbl_gallery").Include("tbl_carModelProduct.tbl_carModel.tbl_carBrand").Where(a => a.statu && a.tbl_stock.Any(b => b.stockCount > 0) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel != null) && a.tbl_carModelProduct.Any(c => c.tbl_carModel.tbl_carBrand != null) && a.langId == langId).OrderByDescending(a => a.sequence).ToList();

            foreach (var item in newProductList)
            {
                searchText = item.name + item.detail + item.shortDescription + item.metaKeyword + item.metaDescription;
                searchText = generateSearchText(searchText, new CultureInfo[] { currentCultureInfo, enCulture });


                if (isMatchSearchWord(searchText, searchWord.ToLower(currentCultureInfo)))
                {
                    searchResultItem searchItem = new searchResultItem();

                    searchItem.objType = searchObjType.product;
                    searchItem.resultTitle = item.name;
                    searchItem.resultSubTitle = item.metaDescription;
                    searchItem.productItem = ps.getProductSummaryOneItem(productUrl, item, currentCultureInfo, langCode, "150", "100");
                    helperList.Add(searchItem);
                }
            }

            #endregion

            #region Brand

            var brandList = db.tbl_carBrand.Where(a => a.statu).ToList();

            foreach (var item in brandList)
            {
                searchText = item.name + item.metaDescription + item.metaKeyword;
                searchText = generateSearchText(searchText, new CultureInfo[] { currentCultureInfo, enCulture });

                if (isMatchSearchWord(searchText, searchWord.ToLower(currentCultureInfo)))
                {
                    searchResultItem searchItem = new searchResultItem();

                    searchItem.objType = searchObjType.brand;
                    searchItem.resultTitle = item.name;
                    searchItem.resultSubTitle = item.metaDescription;
                    searchItem.photo = "ImageShow/carBrand/" + item.photo + "/" + item.photoCoordinate + "/60/60/1";
                    searchItem.url = langCode + "/" + productUrl + "/" + item.url + ".html";

                    helperList.Add(searchItem);
                }
            }

            #endregion

            #region Model

            var modelList = db.tbl_carModel.Include("tbl_carBrand").Where(a => a.statu).ToList();

            foreach (var item in modelList)
            {
                searchText = item.name + item.metaDescription + item.metaKeyword;
                searchText = generateSearchText(searchText, new CultureInfo[] { currentCultureInfo, enCulture });

                if (isMatchSearchWord(searchText, searchWord.ToLower(currentCultureInfo)))
                {
                    searchResultItem searchItem = new searchResultItem();

                    searchItem.objType = searchObjType.model;
                    searchItem.resultTitle = item.name;
                    searchItem.resultSubTitle = item.metaDescription;
                    searchItem.photo = "ImageShow/carBrand/" + item.tbl_carBrand.photo + "/" + item.tbl_carBrand.photoCoordinate + "/60/60/1";
                    searchItem.url = langCode + "/" + productUrl + "/" + item.tbl_carBrand.url + "/" + item.url + ".html";

                    helperList.Add(searchItem);
                }
            }

            #endregion


            #region Page

            var pageList = db.tbl_page.Where(a => a.statu).ToList();

            foreach (var item in pageList)
            {
                searchText = item.name + item.detail + item.metaKeyword + item.metaDescription;
                searchText = generateSearchText(searchText, new CultureInfo[] { currentCultureInfo, enCulture });

                if (isMatchSearchWord(searchText, searchWord.ToLower(currentCultureInfo)))
                {

                    switch ((pageType)item.pageTypeId)
                    {

                        case pageType.activation:

                            continue;

                        case pageType.resetPassword:

                            continue;

                        case pageType.search:

                            continue;

                        case pageType.yonlendirmeIcerik:

                            continue;

                        default:

                            var accountPageList = new pageType[] { pageType.accountDashboard, pageType.accountUserInfo, pageType.accountPassword, pageType.accountAddress, pageType.accountOrders, pageType.accountDiscount };


                            searchResultItem searchItem = new searchResultItem();

                            searchItem.resultTitle = item.name;
                            searchItem.resultSubTitle = item.metaDescription;
                            searchItem.objType = searchObjType.staticPage;

                            if (accountPageList.Contains((pageType)item.pageTypeId))
                            {
                                var accountPage = pas.getPageByType(pageType.account, langId);
                                searchItem.url = langCode + "/" + accountPage.url + "/" + item.url + ".html";
                            }
                            else
                            {
                                searchItem.url = langCode + "/" + item.url + ".html";
                            }

                            helperList.Add(searchItem);

                            break;
                    }
                }

            }


            #endregion



            return helperList;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchModal(string keyWord)
        {
            helperSearchModal helperItem = new helperSearchModal();
            pageShared ps = new pageShared(db);
            productShared pc = new productShared(db);

            var searchPage = ps.getPageByType(pageType.search, langId);
            helperItem.searchLink = Url.Content("~/") + langCode + "/" + searchPage.url + ".html?keyWord=" + keyWord;

            helperItem.searchList = getSearchResultList(keyWord);
            helperItem.totalSearch = helperItem.searchList.Count;

            helperItem.searchList = helperItem.searchList.Take(5).ToList();



            string isResultCome = "no";
            if (helperItem.totalSearch > 0)
            {
                isResultCome = "yes";
            }


            string htmlText = RenderRazorViewToString("SearchModal", helperItem);

            return Json(new { html = htmlText, keywordList = getSearchKeywordList(keyWord), isResultCome = isResultCome });

        }

        public bool isMatchSearchWord(string input, string key)
        {

            var keyList = key.Split(' ').ToList();
            bool result = true;

            foreach (var item in keyList)
            {
                if (result == false)
                {
                    continue;
                }

                if (input.IndexOf(item.ToLower()) == -1)
                {
                    result = false;
                }
            }

            return result;

        }

        public breadCrumb getBreadCrumbStaticPage(string pageName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = pageName;
            helperItem.url = "#";

            return helperItem;
        }

        public string generateSearchText(string searchText, CultureInfo[] cultureInfoList)
        {
            string newText = "";
            foreach (var item in cultureInfoList)
            {
                newText = newText + searchText.ToLower(item);
            }

            return Server.HtmlDecode(newText);
        }

        private List<string> getSearchKeywordList(string keyWord)
        {

            var currentCultureInfo = CultureInfo.CreateSpecificCulture(langCulture);
            var enCulture = CultureInfo.CreateSpecificCulture("en-US");

            List<string> keywordList = keyWord.ToLower(currentCultureInfo).Split(' ').ToList();
            keywordList.AddRange(keyWord.ToLower(enCulture).Split(' ').ToList());

            keywordList = keywordList.Distinct().ToList();

            return keywordList;
        }

    }
}
