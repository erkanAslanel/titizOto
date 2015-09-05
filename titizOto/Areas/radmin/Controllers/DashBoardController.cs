using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class DashBoardController : DbWithControllerWithoutMetod
    {
        public ActionResult Index()
        {
            var item = new DashBoardStatistic(); 

            item.categoryCount = db.tbl_category.Count().ToString();
            item.sliderCount = db.tbl_slider.Count().ToString();

            var os = new HelperSite.Shared.orderShared(db);
            var waitinOrderList = os.getWaitingOrderStatuList();
            item.orderList = db.tbl_order.Where(a => waitinOrderList.Contains(a.orderStatu)).OrderByDescending(a => a.orderId).ToList();

            DateTime nowDate = DateTime.Now;
            DateTime firstDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day);


            item.dayOrder = db.tbl_order.Where(a => a.createDate > firstDate).Count();
            item.dayUser = db.tbl_user.Where(a => a.createDate > firstDate).Count();
            item.dayNewsletter = db.tbl_newsletterUser.Where(a => a.createTime > firstDate).Count();

            item.allOrder = db.tbl_order.Count();
            item.allCategory = db.tbl_category.Count();
            item.allProduct = db.tbl_product.Count();
            item.allPage = db.tbl_page.Count();
            item.allUser = db.tbl_user.Count();
            item.allNewsletter = db.tbl_newsletterUser.Count();

            return View(item);
        }
    }

}
