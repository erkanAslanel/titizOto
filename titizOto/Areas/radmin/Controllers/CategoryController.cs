using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;
using titizOto.Models;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class CategoryController : DbWithControllerWithSorting<tbl_category>
    {
        public override ActionResult Create()
        {
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), null, true, true);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "parentId");
            return base.Create();
        }

        [HttpPost]
        public override ActionResult Create(tbl_category item)
        {
            List<int> selectedList = new List<int>();
            selectedList.Add(item.parentId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "parentId");

            return base.Create(item);
        }

        public override ActionResult Edit(int id)
        {
            tbl_category item = getById(id);
            List<int> selectedList = new List<int>();
            selectedList.Add(item.parentId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "parentId");


            return base.Edit(id);
        }

        [HttpPost]
        public override ActionResult Edit(int id, tbl_category item)
        {
            List<int> selectedList = new List<int>();
            selectedList.Add(item.parentId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "parentId");

            return base.Edit(id, item);
        }

        public override ActionResult GeneralSorting()
        { 
            ViewData["sorting"] = getNestedCategoryHtml();
            return View();
        }

        [HttpPost]
        public override ActionResult GeneralSorting(string sortArray)
        { 
            System.Collections.Generic.List<sortingWithParentId> results = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<sortingWithParentId>>(sortArray);

            results.RemoveAt(0);


            var parentList = results.Where(a => a.parent_id == null).ToList();
            int seqVal = 1;
            int seqSubVal = parentList.Count + 1;
            int seqThird = results.Count() + 1;

            try
            {
                foreach (var item in parentList)
                {
                    int itemCategoryId = int.Parse(item.item_id);

                    updateSequence(itemCategoryId, seqVal);

                    foreach (var subItem in results.Where(a => a.parent_id == item.item_id).ToList())
                    {
                        int subCategoryId = int.Parse(subItem.item_id);
                        updateSequence(subCategoryId, seqSubVal);
                        seqSubVal = seqSubVal + 1;

                        foreach (var subSubItem in results.Where(a => a.parent_id == subItem.item_id).ToList())
                        {
                            subCategoryId = int.Parse(subSubItem.item_id);
                            updateSequence(subCategoryId, seqSubVal);
                            seqSubVal = seqSubVal + 1;

                            foreach (var subSubSubItem in results.Where(a => a.parent_id == subSubItem.item_id).ToList())
                            {
                                subCategoryId = int.Parse(subSubSubItem.item_id);
                                updateSequence(subCategoryId, seqThird);
                                seqThird = seqThird + 1;
                            }
                        }
                    }

                    seqVal = seqVal + 1;
                }

             
                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();
            }
            catch (Exception ex)
            {
                errorSend(ex, " Kategori Genel Sıralama");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            ViewData["sorting"] = getNestedCategoryHtml();

            return View();
        }

        public string getNestedCategoryHtml()
        {
            var parentList = db.tbl_category.Where(a => a.statu == true && a.parentId == 0).OrderBy(a=>a.sequence).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var item in parentList)
            {
                sb.AppendLine(" <li id=\"" + "list_" + item.categoryId.ToString() + "\" class=\"ui-nestedSortable-no-nesting\" data-parentId=\"0\">");
                sb.AppendLine("<div>" + item.name + "</div>");
                sb.AppendLine(getSubNestedCategoryHtml(item.categoryId));
                sb.AppendLine("</li>");
            }

            return sb.ToString();
        }

        public string getSubNestedCategoryHtml(int categoryId)
        {
            var parentList = db.tbl_category.Where(a => a.statu == true && a.parentId == categoryId).OrderBy(a => a.sequence).ToList();

            if (parentList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<ol>");
                foreach (var item in parentList)
                {
                    sb.AppendLine(" <li id=\"" + "list_" + item.categoryId.ToString() + "\" class=\"ui-nestedSortable-no-nesting\" data-parentId=\"" + categoryId.ToString() + "\">");
                    sb.AppendLine("<div>" + item.name + "</div>");
                    sb.AppendLine(getSubNestedCategoryHtml(item.categoryId));
                    sb.AppendLine("</li>");
                }
                sb.AppendLine("</ol>");
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

    }
}
