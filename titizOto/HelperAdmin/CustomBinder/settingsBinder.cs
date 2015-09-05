using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace titizOto.HelperAdmin.CustomBinder
{
    public class settingsBinder : System.Web.Mvc.DefaultModelBinder
    {
        public override object BindModel(System.Web.Mvc.ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            tbl_settings item = ((tbl_settings)base.BindModel(controllerContext, bindingContext));

            HttpRequestBase request = controllerContext.HttpContext.Request; 
            bindingContext.ModelState.Remove("transferDiscountAmount");

            var enUsCulture = CultureInfo.CreateSpecificCulture("en-US");

            string transferDiscountAmount = request.Form.Get("transferDiscountAmount");

            if (string.IsNullOrWhiteSpace(transferDiscountAmount))
            {
                bindingContext.ModelState.AddModelError("transferDiscountAmount", "İndirim Yüzde / İndirim Tutar boş olamaz");
                return item;
            }

            decimal decTransferDiscountAmount = 0;
            if (!decimal.TryParse(transferDiscountAmount, NumberStyles.AllowDecimalPoint, enUsCulture, out decTransferDiscountAmount))
            {
                bindingContext.ModelState.AddModelError("transferDiscountAmount", "İndirim Yüzde / İndirim Tutar alanı uygun formatta değil");
                return item;
            }

            item.transferDiscountAmount = decTransferDiscountAmount;
             
            return item;
        }
        
    }
}