using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.Attribute;
using HelperSite.DbController;
using HelperSite.Shared;
using titizOto.Models;
using ViewModel.Search;
using ViewModel.Shared;

namespace titizOto.Controllers
{
    public class SharedController : DbWithControllerWithMaster
    {
        [OutputCache(VaryByCustom = "tbl_page;tbl_category", Duration = 300)]
        public PartialViewResult topMenu()
        {
            var categoryList = db.tbl_category.Include("tbl_page").Where(a => a.statu && a.langId == langId && a.isMainMenuShown && a.parentId == 0).OrderBy(a => a.sequence).ToList();

            var list = getTopMenu(langCode, categoryList, mainPath);

            return PartialView(list);

        }

        [OutputCache(VaryByCustom = "tbl_page;tbl_category", Duration = 300)]
        public PartialViewResult footerMenu()
        {
            var categoryList = db.tbl_category.Include("tbl_page").Where(a => a.statu && a.langId == langId && a.isFooterMenuShown).OrderBy(a => a.sequence).ToList();
            var list = getFooterMenu(langCode, categoryList, mainPath);

            return PartialView(list);

        }

        public List<topMenu> getTopMenu(string langCode, List<tbl_category> list, string mainPath)
        {
            pageShared ps = new pageShared(db);
            List<topMenu> helperList = new List<topMenu>();

            foreach (var item in list)
            {
                topMenu helperItem = new topMenu();

                helperItem.name = item.name;

                var page = item.tbl_page;

                if (page != null && page.Count != 0)
                {
                    var pageItem = item.tbl_page.FirstOrDefault();
                    helperItem.url = mainPath + langCode + "/" + ps.getPageUrl(pageItem) + ".html";
                }
                else
                {
                    helperItem.url = "#";
                }

                helperList.Add(helperItem);
            }

            return helperList;

        }

        public List<footerMenu> getFooterMenu(string langCode, List<tbl_category> list, string mainPath)
        {
            List<footerMenu> helperList = new List<footerMenu>();

            var parentList = list.Where(a => a.parentId == 0).ToList();

            foreach (var item in parentList)
            {


                footerMenu helperItem = new footerMenu();

                helperItem.name = item.name;

                if (item.tbl_page == null || item.tbl_page.FirstOrDefault() == null)
                {
                    helperItem.url = "#";
                    helperItem.hiddenUrl = mainPath + langCode;
                }
                else
                {
                    var pageItem = item.tbl_page.FirstOrDefault();
                    helperItem.hiddenUrl = mainPath + langCode + "/" + pageItem.url;

                    // Geçiş Kategorisi Doğrudan Link Kullanılmıyor.
                    if (pageItem.isHelperUrl)
                    {
                        helperItem.url = "#";
                    }
                    else
                    {
                        if (pageItem.pageTypeId == (int)pageType.yonlendirmeIcerik)
                        {
                            helperItem.url = mainPath + pageItem.redirectPageUrl;
                        }
                        else
                        {
                            helperItem.url = mainPath + langCode + "/" + pageItem.url + ".html";
                        }
                    }
                }

                // alt Kategoriler
                var subCategory = list.Where(a => a.parentId == item.categoryId).ToList();
                if (subCategory.Count > 0)
                {
                    helperItem.children = new List<footerMenu>();

                    foreach (var subItem in subCategory)
                    {
                        footerMenu subHelperItem = new footerMenu();

                        subHelperItem.name = subItem.name;

                        if (subItem.tbl_page == null || subItem.tbl_page.FirstOrDefault() == null)
                        {
                            subHelperItem.url = "#";
                        }
                        else
                        {
                            var subPageItem = subItem.tbl_page.FirstOrDefault();
                            subHelperItem.hiddenUrl = helperItem.hiddenUrl + "/" + subPageItem.url;

                            // Geçiş Kategorisi Doğrudan Link Kullanılmıyor.
                            if (subPageItem.isHelperUrl)
                            {
                                subHelperItem.url = "#";
                            }
                            else
                            {
                                if (subPageItem.pageTypeId == (int)pageType.yonlendirmeIcerik)
                                {
                                    subHelperItem.url = mainPath + subPageItem.redirectPageUrl;
                                }
                                else
                                {
                                    subHelperItem.url = subHelperItem.hiddenUrl + ".html";
                                }
                            }
                        }

                        helperItem.children.Add(subHelperItem);

                    }

                }


                helperList.Add(helperItem);

            }

            return helperList;
        }

        [OutputCache(VaryByCustom = "tbl_page", Duration = 300)]
        public PartialViewResult search()
        {
            pageShared ps = new pageShared(db);
            var searchPage = ps.getPageByType(pageType.search, langId);

            var helperItem = new searchKey();
            helperItem.searchLink = Url.Content("~/") + langCode + "/" + searchPage.url + ".html";

            return PartialView(helperItem);

        }

    }
}
