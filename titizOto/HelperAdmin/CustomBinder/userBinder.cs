using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using titizOto.Models;

namespace titizOto.HelperAdmin.CustomBinder
{
    public class userBinder : System.Web.Mvc.DefaultModelBinder
    {
        public override object BindModel(System.Web.Mvc.ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            tbl_user item = ((tbl_user)base.BindModel(controllerContext, bindingContext));

            bindingContext.ModelState.Remove("email");

            if (string.IsNullOrWhiteSpace(item.email))
            {
                bindingContext.ModelState.AddModelError("email", "Email girişini kontrol ediniz.");
                return item;
            }

            try
            {
                MailAddress mail = new MailAddress(item.email);
            }
            catch
            { 
                bindingContext.ModelState.AddModelError("email", "Email girişini kontrol ediniz.");
                return item;
            }


            return item;

        }

    }
}