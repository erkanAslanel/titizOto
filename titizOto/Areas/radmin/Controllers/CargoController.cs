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
    public class CargoController : DbWithControllerWithSorting<tbl_cargo>
    {
         

    }
}
