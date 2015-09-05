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
    public class PosOptionController : DbWithControllerWithSortingWithFilter<tbl_bankPosOption>
    {
        public override List<tbl_bankPosOption> getListWithFilter(int filterId)
        {
            return base.getListWithFilter(filterId).Where(a => a.bankPosId == filterId).OrderBy(a => a.paymentCount).ToList();
        }

    }
}
