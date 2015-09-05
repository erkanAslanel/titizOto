using titizOto.Models;
using HelperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class AdminUserController : DbWithController<tbl_adminUser>
    {
        public override ActionResult Create()
        {
            tbl_adminUser item = new tbl_adminUser();
            item.enterDate = DateTime.Now;
            item.adminRoleId = 1;
            return View(item);
        }

        public override ActionResult delete(int id)
        {
            var item = getById(id);
            if (item.adminRoleId == 1)
            {
                return View("Table", getList());
            }
            else
            {
                return base.delete(id);
            }
        }

        public override ActionResult setDeleteAll(FormCollection formCollection)
        {
            int id = 0;

            List<string> idList = formCollection["selectedItem"].Split(',').Where(a => a != "false" && a != "true").ToList();
            foreach (var item in idList)
            {
                id = int.Parse(item);

                var objectItem = getById(id);
                if (objectItem.adminRoleId!=1)
                {
                    deleteItem(objectItem);
                }
              
            }

            return View("Table", getList());
        }

    }
}
