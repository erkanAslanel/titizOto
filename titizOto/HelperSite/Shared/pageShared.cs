using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelperSite;
using HelperSite.Interface;
using titizOto.Models;

namespace HelperSite.Shared
{
    public class pageShared : baseShared
    {
        public pageShared(titizOtoEntities db)
        {
            this.db = db;
        }

        public tbl_page getPageById(int pageId)
        {
            return db.tbl_page.Where(a => a.pageId == pageId).FirstOrDefault();
        }

        public string getPageUrl(tbl_page pageItem)
        {
            string url = null;

            if (pageItem != null)
            {
                if (pageItem.isHelperUrl)
                {
                    url = "#";
                }
                else
                {
                    if (pageItem.pageTypeId == (int)pageType.yonlendirmeIcerik && !string.IsNullOrWhiteSpace(pageItem.redirectPageUrl))
                    {
                        url = pageItem.redirectPageUrl;
                    }
                    else
                    {
                        url = pageItem.url;
                    }
                }
            }

            return url;

        }

        public void pageTitleBind(tbl_page pageItem, IPageable item, int langId)
        {

            if (pageItem != null)
            {
                pageTitleBind(pageItem.name, pageItem.title, pageItem.metaDescription, pageItem.metaDescription, pageItem.pageId, item, langId);
            }
        }

        public void pageTitleBind(string name, string browserTitle, string metaDescription, string metaKeyword, int pageId, IPageable item, int langId)
        {
            var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();

            if (settingItem != null)
            {
                item.setTitle(name);
                item.setBrowserTitle(name + settingItem.allPageTitle);

                if (!string.IsNullOrWhiteSpace(browserTitle))
                {
                    item.setBrowserTitle(browserTitle + settingItem.allPageTitle);
                }

                if (!string.IsNullOrWhiteSpace(metaDescription))
                {
                    item.setDescription(metaDescription);
                }

                if (!string.IsNullOrWhiteSpace(metaKeyword))
                {
                    item.setKeywords(metaKeyword);
                }

                item.setPageId(pageId);
            }


        }

        public tbl_page getPageByType(pageType pagetype, int langId)
        {
            return db.tbl_page.Where(a => a.pageTypeId == (int)pagetype && a.langId == langId).FirstOrDefault();
        }

        public tbl_module getModuleByType(moduleType moduleType, int langId)
        {
            return db.tbl_module.Where(a => a.typeId == (int)moduleType && a.langId == langId).FirstOrDefault();
        
        }

    }

    public enum pageType
    {
        normalIcerik = 0,
        yonlendirmeIcerik = 1,
        allBrand = 2,
        productList = 3,
        openContent = 4,

        // system
        registerLogin = 5,
        activation = 6,
        activationResent = 7,
        basket = 8,
        forgetPassword = 9,
        resetPassword = 10,
        account = 12,
        accountDashboard = 11,
        accountUserInfo = 13,
        accountPassword = 14,
        accountAddress = 15,
        accountOrders = 16,
        accountOrderDetail = 28,
        accountOrderSearch = 29,
        accountDiscount = 17,
        search = 18,
        checkoutRegisterStatu = 19,
        checkoutDelivery = 20,
        checkoutBilling = 21,
        checkoutCargo = 22,
        checkoutPayment = 23,
        checkoutSummary = 24,
        checkoutMain = 25,
        checkoutComplete = 26,
        checkoutErrorProcess = 27


    }

    public enum moduleType
    {
        standart = 0,
        salesAgreement = 1,
        preSalesAgreement = 2,
        registerAgreement = 3

    }
}