using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;
using titizOto.Models;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class OpenContentController : DbWithControllerWithSorting<tbl_openContent>
    {
        public override ActionResult Create()
        {
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), null, true, true);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "categoryId");
            return base.Create();
        }

        [HttpPost]
        public override ActionResult Create(tbl_openContent item)
        {
            List<int> selectedList = new List<int>();
            selectedList.Add(item.categoryId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "categoryId");

            return base.Create(item);
        }

        public override ActionResult Edit(int id)
        {
            tbl_openContent item = getById(id);
            List<int> selectedList = new List<int>();
            selectedList.Add(item.categoryId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "categoryId");


            return base.Edit(id);
        }

        [HttpPost]
        public override ActionResult Edit(int id, tbl_openContent item)
        {
            List<int> selectedList = new List<int>();
            selectedList.Add(item.categoryId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "categoryId");

            return base.Edit(id, item);
        }

    }
}
