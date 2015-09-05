using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;
using titizOto.App_GlobalResources;
using titizOto.Models;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class AddressController : DbWithControllerWithSortingWithFilter<tbl_address>
    {
        public override List<tbl_address> getListWithFilter(int filterId)
        {
            return base.getListWithFilter(filterId).Where(a => a.userId == filterId && a.isGuestUser == false).OrderBy(a => a.sequence).ToList();
        }

        [HttpPost]
        public override ActionResult Create(tbl_address item)
        {
            // Tc , Tax  ect... 
            extraValidation(item);

            return base.Create(item);
        }

        [HttpPost]
        public override ActionResult Edit(int id, tbl_address item)
        {
            // Tc , Tax  ect... 
            extraValidation(item);

            return base.Edit(id, item);
        }

        private void extraValidation(tbl_address item)
        {

            if (item.isPersonal)
            {
                string tcPattern = @"^\d{11}$";

                if (string.IsNullOrWhiteSpace(item.tcNo) || !Regex.IsMatch(item.tcNo, tcPattern))
                {
                    ModelState.AddModelError("tcNo", lang.addressTcNoRequired);
                }
            }
            else
            {
                string taxNoPattern = @"^\d{10}$";

                if (string.IsNullOrWhiteSpace(item.taxNo) || !Regex.IsMatch(item.taxNo, taxNoPattern))
                {
                    ModelState.AddModelError("taxNo", lang.addressTaxNoRequired);
                }

                if (string.IsNullOrWhiteSpace(item.taxOffice))
                {
                    ModelState.AddModelError("taxOffice", lang.addressTaxOfficeRequired);
                }

                if (string.IsNullOrWhiteSpace(item.companyName))
                {
                    ModelState.AddModelError("taxOffice", lang.addressCompanyNameRequired);
                }
            }

        }

    }
}
