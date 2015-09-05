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
    public class StockController : DbWithControllerWithSortingWithFilter<tbl_stock>
    {
        public override List<tbl_stock> getListWithFilter(int filterId)
        {
            return base.getListWithFilter(filterId).Where(a => a.productId == filterId).OrderBy(a => a.sequence).ToList();
        }

        public override ActionResult Edit(int id, tbl_stock item)
        {
            string optionIdList = null;
            if (Request.Form["optionIdList"] != null)
            {
                optionIdList = Request.Form["optionIdList"];
            }

            item.optionList = optionIdList;

            var stockWithCritear = db.tbl_stock.Where(a => a.productId == item.productId && a.optionList == item.optionList && a.stockId != id).Count();

            if (stockWithCritear > 0)
            {
                ViewBag.success = false;
                ViewBag.resultHtml = getNotification("Seçtiğiniz Ürün Seçeneği İle Stok Mevcuttur.", "Information", "mb10");
                return View(item);
            }
            else
            {
                return base.Edit(id, item);
            }

        }

        public override ActionResult Create(tbl_stock item)
        {
            string optionIdList = null;
            if (Request.Form["optionIdList"] != null)
            {
                optionIdList = Request.Form["optionIdList"];
            }

            item.optionList = optionIdList;


            var stockWithCritear = db.tbl_stock.Where(a => a.productId == item.productId && a.optionList == item.optionList).Count();

            if (stockWithCritear > 0)
            {
                ViewBag.success = false;
                ViewBag.resultHtml = getNotification("Seçtiğiniz Ürün Seçeneği İle Stok Mevcuttur.", "Information", "mb10");
                return View(item);
            }
            else
            {
                return base.Create(item);
            }


        }


    }
}
