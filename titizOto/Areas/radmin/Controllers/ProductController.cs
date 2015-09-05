using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;
using titizOto.Models;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class ProductController : DbWithControllerWithSorting<tbl_product>
    {
        public override ActionResult Create()
        {
            var categoryJson = getProductModelListData(db.tbl_carBrand.ToList(), new List<int>());
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Multi, "productModelList");
            var item = new tbl_product();

            item.isTaxInclude = true;
            item.taxPercent = 18; 

            return View(item);
        }

        [HttpPost]
        public override ActionResult Create(tbl_product item)
        {
            List<int> selectedList = new List<int>();

            if (Request.Form["productModelList"] != null && !string.IsNullOrWhiteSpace(Request.Form["productModelList"].ToString()))
            {
                string productModelList = Request.Form["productModelList"].ToString();
                var strProductModelList = productModelList.Split(',');

                foreach (var strModelId in strProductModelList)
                {
                    int helperInt = 0;

                    if (int.TryParse(strModelId, out helperInt))
                    {
                        selectedList.Add(helperInt);
                    }
                }

            }


            var categoryJson = getProductModelListData(db.tbl_carBrand.ToList(), selectedList);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Multi, "productModelList");

            if (Request.Form["imageclear"] != null)
            {
                ModelState.Clear();
                return View(item);
            }

            if (!isValidUrlSubmitCreate(item))
            {
                ModelState.AddModelError("url", "Seçtiğiniz adres kullanılmaktadır.");
            }


            if (!ModelState.IsValid)
            {
                return View(item);
            }




            try
            {
                add(item);
                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();
                cacheUpdate();
                addProductModel(selectedList, item.productId);

            }
            catch (Exception ex)
            {
                errorSend(ex, item.GetType().Name + " Ekleme İşlemi");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            return View(item);
        }

        public override ActionResult Edit(int id)
        {
            

            List<int> selectedList = db.tbl_carModelProduct.Where(a=>a.productId==id).Select(a => a.carModelId).ToList();

            if (selectedList == null)
            {
                selectedList = new List<int>();
            }


            var categoryJson = getProductModelListData(db.tbl_carBrand.ToList(), selectedList);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Multi, "productModelList");

            return base.Edit(id);
        }

        [HttpPost]
        public override ActionResult Edit(int id, tbl_product item)
        {


            List<int> selectedList = new List<int>();

            if (Request.Form["productModelList"] != null && !string.IsNullOrWhiteSpace(Request.Form["productModelList"].ToString()))
            {
                string productModelList = Request.Form["productModelList"].ToString();
                var strProductModelList = productModelList.Split(',');

                foreach (var strModelId in strProductModelList)
                {
                    int helperInt = 0;

                    if (int.TryParse(strModelId, out helperInt))
                    {
                        selectedList.Add(helperInt);
                    }
                }

            }


            var categoryJson = getProductModelListData(db.tbl_carBrand.ToList(), selectedList);
            ViewBag.treeView = new TreeviewOption(jsonToHtmlString(categoryJson), SelectionMode.Multi, "productModelList");

            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("sequence");
            prop.SetValue(item, prop.GetValue(getById(id), null), null);


            if (Request.Form["imageclear"] != null)
            {
                ModelState.Clear();
                return View(item);
            }

            if (!isValidUrlSubmitEdit(item, id))
            {
                ModelState.AddModelError("url", "Seçtiğiniz adres kullanılmaktadır.");
            }

            if (!isValidUrlSubmitEditEn(item, id))
            {
                ModelState.AddModelError("urlEn", "Seçtiğiniz adres kullanılmaktadır.(İngilizce)");
            }

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            try
            {
                editItem(id, item);
                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();
                cacheUpdate();
                addProductModel(selectedList, item.productId);
            }
            catch (Exception ex)
            {
                errorSend(ex, item.GetType().Name + " Editleme İşlemi");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            return View(item);
        }

        private System.Web.Mvc.JsonResult getProductModelListData(List<tbl_carBrand> carBrandList, List<int> selectedIdList)
        {
            List<HierarchyData> dataList = new List<HierarchyData>();


            foreach (var item in carBrandList)
            {
                dataList.Add(new HierarchyData(item.name, item.carBrandId, getSubModelByBrandId(item, selectedIdList), false, "lang" + item.langId + " dynatree-noselectable"));

            }

            return Json(dataList, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        private List<HierarchyData> getSubModelByBrandId(tbl_carBrand parentItem, List<int> selectedIdlist)
        {
            List<HierarchyData> dataList = new List<HierarchyData>();
            var modelList = parentItem.tbl_carModel.ToList();

            foreach (var item in modelList)
            {
                bool isSelect = false;

                if (selectedIdlist.Contains(item.carModelId))
                {
                    isSelect = true;
                }

                dataList.Add(new HierarchyData(item.name, item.carModelId, null, isSelect, "lang" + item.langId));
            }

            return dataList;
        }

        private void addProductModel(List<int> selectedList, int productId)
        {
            var deleteList = db.tbl_carModelProduct.Where(a => a.productId == productId).ToList();

            foreach (var item in deleteList)
            {
                db.tbl_carModelProduct.Remove(item);
            }

            int addIndex = 1;
            foreach (var item in selectedList)
            {
                tbl_carModelProduct addItem = new tbl_carModelProduct();

                addItem.carModelId = item;
                addItem.productId = productId;
                addItem.sequence = addIndex;
                db.tbl_carModelProduct.Add(addItem);
                addIndex++;

            }

            db.SaveChanges();

        }

        public override void deleteItem(tbl_product item)
        {
            base.updateItemIsDeleted(item);
        }

        public override List<tbl_product> getList()
        {
            return base.getList().Where(a => a.isDeleted != true).ToList();
        }

    }
}
