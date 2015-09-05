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
    public class CritearController : DbWithControllerWithSorting<tbl_critear>
    {
        public override ActionResult Create()
        {
            var categoryJson = getCritearListData(db.tbl_critear.ToList(), null, true, true);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "parentId");
            return base.Create();
        }

        [HttpPost]
        public override ActionResult Create(tbl_critear item)
        {
            List<int> selectedList = new List<int>();
            selectedList.Add(item.parentId);
            var categoryJson = getCritearListData(db.tbl_critear.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "parentId");

            return base.Create(item);
        }

        public override ActionResult Edit(int id)
        {
            tbl_critear item = getById(id);
            List<int> selectedList = new List<int>();
            selectedList.Add(item.parentId);
            var categoryJson = getCritearListData(db.tbl_critear.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "parentId");


            return base.Edit(id);
        }

        [HttpPost]
        public override ActionResult Edit(int id, tbl_critear item)
        {
            List<int> selectedList = new List<int>();
            selectedList.Add(item.parentId);
            var categoryJson = getCategoryListData(db.tbl_category.ToList(), selectedList, true, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "parentId");

            return base.Edit(id, item);
        }


        public System.Web.Mvc.JsonResult getCritearListData(List<tbl_critear> categoryList, List<int> selectedIdList, bool isRootAdded, bool isRootSelected)
        {
            List<HierarchyData> dataList = new List<HierarchyData>();

            //Root gerçekten Seçiliyse, 
            if (selectedIdList != null && selectedIdList.Contains(0))
            {
                dataList.Add(new HierarchyData("Root", 0, null, true, "root"));
            }
            else // Root startup Control
            {
                if (isRootAdded)
                {
                    dataList.Add(new HierarchyData("Root", 0, null, isRootSelected, "root"));
                }
            }

            dataList.AddRange(recursiveCritearList(0, categoryList, selectedIdList));

            return Json(dataList, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        } 

        private List<HierarchyData> recursiveCritearList(int parentId, List<tbl_critear> categoryList, List<int> selectedIdlist)
        {
            List<HierarchyData> dataList = new List<HierarchyData>();

            List<tbl_critear> categoryListFilter = categoryList.Where(a => a.parentId == parentId).OrderBy(a => a.sequence).ToList();

            foreach (tbl_critear item in categoryListFilter)
            {
                HierarchyData dataItem = null;

                //Selected Data
                if (selectedIdlist != null && selectedIdlist.Contains(item.critearId))
                {
                    //dataItem = new HierarchyData(item.name, item.categoryId, recursiveCategoryList(item.categoryId, categoryList, selectedIdlist), true, "lang" + item.langId.ToString());
                    dataItem = new HierarchyData(item.name, item.critearId, recursiveCritearList(item.critearId, categoryList, selectedIdlist), true, "lang" + 1);
                }
                else
                {
                    //dataItem = new HierarchyData(item.name, item.categoryId, recursiveCategoryList(item.categoryId, categoryList, selectedIdlist), false, "lang" + item.langId.ToString());
                    dataItem = new HierarchyData(item.name, item.critearId, recursiveCritearList(item.critearId, categoryList, selectedIdlist), false, "lang" + 1);

                }

                dataList.Add(dataItem);
            }

            return dataList;
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
                errorSend(ex, " Ürün Seçenek Genel Sıralama");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            ViewData["sorting"] = getNestedCategoryHtml();

            return View();
        }

        public string getNestedCategoryHtml()
        {
            var parentList = db.tbl_critear.Where(a => a.parentId == 0).OrderBy(a => a.sequence).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var item in parentList)
            {
                sb.AppendLine(" <li id=\"" + "list_" + item.critearId.ToString() + "\" class=\"ui-nestedSortable-no-nesting\" data-parentId=\"0\">");
                sb.AppendLine("<div>" + item.name + "</div>");
                sb.AppendLine(getSubNestedCategoryHtml(item.critearId));
                sb.AppendLine("</li>");
            }

            return sb.ToString();
        }

        public string getSubNestedCategoryHtml(int categoryId)
        {
            var parentList = db.tbl_critear.Where(a => a.statu == true && a.parentId == categoryId).OrderBy(a => a.sequence).ToList();

            if (parentList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<ol>");
                foreach (var item in parentList)
                {
                    sb.AppendLine(" <li id=\"" + "list_" + item.critearId.ToString() + "\" class=\"ui-nestedSortable-no-nesting\" data-parentId=\"" + categoryId.ToString() + "\">");
                    sb.AppendLine("<div>" + item.name + "</div>");
                    sb.AppendLine(getSubNestedCategoryHtml(item.critearId));
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

        public override void deleteItem(tbl_critear item)
        {
            base.updateItemIsDeleted(item);
        }

        public override List<tbl_critear> getList()
        {
            return base.getList().Where(a => a.isDeleted != true).ToList();
        }

    }
}
