using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace titizOto.HelperAdmin.CustomBinder
{
    public class cargoBinder : System.Web.Mvc.DefaultModelBinder
    { 
        public override object BindModel(System.Web.Mvc.ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            tbl_cargo item = ((tbl_cargo)base.BindModel(controllerContext, bindingContext));

            HttpRequestBase request = controllerContext.HttpContext.Request;

            bindingContext.ModelState.Remove("price");
            bindingContext.ModelState.Remove("discountPrice");

            var enUsCulture = CultureInfo.CreateSpecificCulture("en-US");

            string price = request.Form.Get("price");

            if (string.IsNullOrWhiteSpace(price))
            {
                bindingContext.ModelState.AddModelError("price", "Fiyat boş olamaz");
                return item;
            }

            decimal decPrice = 0;
            if (!decimal.TryParse(price, NumberStyles.AllowDecimalPoint, enUsCulture, out decPrice))
            {
                bindingContext.ModelState.AddModelError("price", "Fiyat alanı uygun formatta değil");
                return item;
            }

            item.price = decPrice;

            string freeCargoPrice = request.Form.Get("freeCargoPrice");

            if (string.IsNullOrWhiteSpace(freeCargoPrice))
            {
                bindingContext.ModelState.AddModelError("freeCargoPrice", "Bedava Kargo Sepet Tutarı boş olamaz");
            }

            decimal decFreeCargoPrice = 0;
            if (!decimal.TryParse(freeCargoPrice, NumberStyles.AllowDecimalPoint, enUsCulture, out decFreeCargoPrice))
            {
                bindingContext.ModelState.AddModelError("freeCargoPrice", "Bedava Kargo Sepet Tutarı alanı uygun formatta değil");
                return item;
            }

            item.freeCargoPrice = decFreeCargoPrice;

            return item;
        }
         
    }




}