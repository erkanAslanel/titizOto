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
    public class ProductCritearController : DbWithControllerWithSortingWithFilter<tbl_productCritear>
    {
        public override List<tbl_productCritear> getListWithFilter(int filterId)
        {
            return base.getListWithFilter(filterId).Where(a => a.productId == filterId).OrderBy(a => a.sequence).ToList();
        }

        public override ActionResult Create()
        {
            var categoryJson = getCritearListData(db.tbl_critear.ToList(), null, false, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "critearId");
            return base.Create();
        }

        [HttpPost]
        public override ActionResult Create(tbl_productCritear item)
        {
            List<int> selectedList = new List<int>();
            selectedList.Add(item.critearId);
            var categoryJson = getCritearListData(db.tbl_critear.ToList(), selectedList, false, false);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Single, "critearId");

            ViewBag.success = false;
            ViewBag.resultHtml = getNotificationDefaultError();

            var critearListCount = db.tbl_productCritear.Where(a => a.productId == item.productId && a.critearId == item.critearId).Count();

            if (critearListCount > 0)
            {
                ViewBag.success = false;
                ViewBag.resultHtml = getNotification("Seçtiğiniz Ürün Seçeneği Üründe Mevcuttur.", "Information","mb10");
                return View(item);
            }
            else
            {
                return base.Create(item);
            }

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


                    if (parentId == 0)
                    {
                        dataItem = new HierarchyData(item.name, item.critearId, recursiveCritearList(item.critearId, categoryList, selectedIdlist), true, "lang" + 1 + " dynatree-noselectable");
                    }
                    else
                    {
                        dataItem = new HierarchyData(item.name, item.critearId, recursiveCritearList(item.critearId, categoryList, selectedIdlist), true, "lang" + 1);
                    }

                }
                else
                {
                    //dataItem = new HierarchyData(item.name, item.categoryId, recursiveCategoryList(item.categoryId, categoryList, selectedIdlist), false, "lang" + item.langId.ToString());


                    if (parentId == 0)
                    {
                        dataItem = new HierarchyData(item.name, item.critearId, recursiveCritearList(item.critearId, categoryList, selectedIdlist), false, "lang" + 1 + " dynatree-noselectable");
                    }
                    else
                    {
                        dataItem = new HierarchyData(item.name, item.critearId, recursiveCritearList(item.critearId, categoryList, selectedIdlist), false, "lang" + 1);
                    }

                }

                dataList.Add(dataItem);
            }

            return dataList;
        }

    }
}
