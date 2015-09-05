 
using HelperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.Models;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class ModuleController : DbWithController<tbl_module>
    {
        public override ActionResult Create(tbl_module item)
        {
            item.tag = "";
            base.Create(item);

            item.tag = "{" + item.moduleId.ToString() + "," + item.name + "}";
            db.SaveChanges();

            return View(item);
        }

        public override ActionResult Edit(int id, tbl_module item)
        {
            if (ModelState.IsValid)
            {
                item.tag = "{" + item.moduleId.ToString() + "," + item.name + "}";
            }

            base.Edit(id, item);

            db.SaveChanges();

            return View(item);
        }

    }
}
