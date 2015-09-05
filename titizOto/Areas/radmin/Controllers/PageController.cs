using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;
using titizOto.Models;
using HelperSite.Shared;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class PageController : DbWithControllerWithSorting<tbl_page>
    {
        public override ActionResult Create()
        {
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), null, true, true);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "categoryId");
            return base.Create();
        }

        [HttpPost]
        public override ActionResult Create(tbl_page item)
        {
            List<int> selectedList = new List<int>();
            selectedList.Add(item.categoryId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "categoryId");

            return base.Create(item);
        }

        public override ActionResult Edit(int id)
        {
            tbl_page item = getById(id);
            List<int> selectedList = new List<int>();
            selectedList.Add(item.categoryId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "categoryId");


            return base.Edit(id);
        }

        [HttpPost]
        public override ActionResult Edit(int id, tbl_page item)
        {
            List<int> selectedList = new List<int>();
            selectedList.Add(item.categoryId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "categoryId");

            return base.Edit(id, item);
        }

        public ActionResult SystemPage()
        {
            return View(getSystemPage(getList()));
        }

        public override ActionResult Index()
        {
            return View(getSystemPageWithout(getList()));
        }

        public override ActionResult GeneralSorting()
        {
            var list = getSystemPageWithout(getList());

            if (list.Count == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(list);
            }
        }

        // No-System Page 
        private List<tbl_page> getSystemPageWithout(List<tbl_page> list)
        {
            return list.Where(a => !getSystemTypeList().Contains(a.pageTypeId)).ToList();
        }

        // System Page
        private List<tbl_page> getSystemPage(List<tbl_page> list)
        {
            return list.Where(a => getSystemTypeList().Contains(a.pageTypeId)).ToList();
        }

        private List<int> getSystemTypeList()
        {
            var item = new List<int>();

            item.Add((int)pageType.activation);
            item.Add((int)pageType.activationResent);
            item.Add((int)pageType.basket);
            item.Add((int)pageType.forgetPassword);
            item.Add((int)pageType.registerLogin);
            item.Add((int)pageType.resetPassword);
            item.Add((int)pageType.account);
            item.Add((int)pageType.accountDashboard);
            item.Add((int)pageType.accountAddress);
            item.Add((int)pageType.accountDiscount);
            item.Add((int)pageType.accountOrders);
            item.Add((int)pageType.accountOrderDetail);
            item.Add((int)pageType.accountOrderSearch);
            item.Add((int)pageType.accountPassword);
            item.Add((int)pageType.accountUserInfo);
            item.Add((int)pageType.search);
            item.Add((int)pageType.checkoutBilling);
            item.Add((int)pageType.checkoutCargo);
            item.Add((int)pageType.checkoutDelivery);
            item.Add((int)pageType.checkoutPayment);
            item.Add((int)pageType.checkoutRegisterStatu);
            item.Add((int)pageType.checkoutSummary);
            item.Add((int)pageType.checkoutComplete);
            item.Add((int)pageType.checkoutErrorProcess);

            return item;

        }

    }
}
