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
    public class GalleryController : DbWithControllerWithSortingWithFilter<tbl_gallery>
    {
        public override List<tbl_gallery> getListWithFilter(int filterId)
        {
            return base.getListWithFilter(filterId).Where(a => a.productId == filterId).OrderBy(a => a.sequence).ToList();
        }


        public override ActionResult Create()
        {
          

            var item = new tbl_gallery(); 
            item.guid = Guid.NewGuid().ToString(); 

            return View(item);

        }

        public override ActionResult Create(tbl_gallery item)
        {
            string optionIdList = null;
            if (Request.Form["optionIdList"] != null)
            {
                optionIdList = Request.Form["optionIdList"];
            }

            item.optionList = optionIdList;    
            
            return base.Create(item);
        }

        public override ActionResult Edit(int id, tbl_gallery item)
        {
            string optionIdList = null;
            if (Request.Form["optionIdList"] != null)
            {
                optionIdList = Request.Form["optionIdList"];
            }

            item.optionList = optionIdList;

            return base.Edit(id, item);
        }
    }
}
