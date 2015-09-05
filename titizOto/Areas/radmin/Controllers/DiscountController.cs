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
    public class DiscountController : DbWithController<tbl_discount>
    {
        public override ActionResult Create()
        {
            var item = new tbl_discount();

            item.startDate = DateTime.Now;
            item.endDate = item.startDate.AddMonths(1);

            return View(item);
        }

    }
}
