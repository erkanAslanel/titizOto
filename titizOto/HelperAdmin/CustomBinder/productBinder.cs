using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.Models;


namespace titizOto.HelperAdmin.CustomBinder
{
    public class productBinder : System.Web.Mvc.DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            tbl_product item = ((tbl_product)base.BindModel(controllerContext, bindingContext));

            HttpRequestBase request = controllerContext.HttpContext.Request;

            var enUsCulture = CultureInfo.CreateSpecificCulture("en-US");

            bindingContext.ModelState.Remove("price");
            bindingContext.ModelState.Remove("discountPrice");
            


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

            string discountPrice = request.Form.Get("discountPrice");

            if (string.IsNullOrWhiteSpace(discountPrice))
            {
                bindingContext.ModelState.AddModelError("discountPrice", "İndirimli fiyat boş olamaz");
            }

            decimal decDiscountPrice = 0;
            if (!decimal.TryParse(discountPrice, NumberStyles.AllowDecimalPoint, enUsCulture, out decDiscountPrice))
            {
                bindingContext.ModelState.AddModelError("discountPrice", "İndirimli fiyat alanı uygun formatta değil");
                return item;
            }

            item.discountPrice = decDiscountPrice;

            return item;
        }
    }
}