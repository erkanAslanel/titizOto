using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HelperAdmin;

namespace titizOto.HelperAdmin.CustomBinder
{
    public class bankPosOptionBinder : System.Web.Mvc.DefaultModelBinder
    {
        public override object BindModel(System.Web.Mvc.ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        { 
            Models.tbl_bankPosOption item = ((Models.tbl_bankPosOption)base.BindModel(controllerContext, bindingContext));

            HttpRequestBase request = controllerContext.HttpContext.Request;

            var enUsCulture = CultureInfo.CreateSpecificCulture("en-US");
            var trCulture = CultureInfo.CreateSpecificCulture("tr-TR");

            DbWithBasicFunction dbc = new DbWithBasicFunction();

            bindingContext.ModelState.Remove("paymentCount");
            bindingContext.ModelState.Remove("minBasketAmount");
            bindingContext.ModelState.Remove("additionalAmount"); 

            if (dbc.db.tbl_bankPosOption.Where(a => a.bankPosId == item.bankPosId && a.bankPosOptionId != item.bankPosOptionId && a.paymentCount == item.paymentCount).Count() > 0)
            {
                bindingContext.ModelState.AddModelError("paymentCount", "Taksit sayısı işlem yapılan sanal pos için mevcut.Başka bir taksit sayısı giriniz.");
                return item;
            }

            #region minBasketAmount

            string minBasketAmount = request.Form.Get("minBasketAmount");
            decimal decMinBasketAmount = 0;
            if (!decimal.TryParse(minBasketAmount, NumberStyles.AllowDecimalPoint, enUsCulture, out decMinBasketAmount))
            {
                bindingContext.ModelState.AddModelError("minBasketAmount", "Uygun formatta değil. Örnek giriş 99.99");
                return item;
            }

            item.minBasketAmount = decMinBasketAmount;

            #endregion

            #region additionalAmount

            string additionalAmount = request.Form.Get("additionalAmount");
            decimal decAdditionalAmount = 0;
            if (!decimal.TryParse(additionalAmount, NumberStyles.AllowDecimalPoint, enUsCulture, out decAdditionalAmount))
            {
                bindingContext.ModelState.AddModelError("additionalAmount", "Uygun formatta değil. Örnek giriş 99.99");
                return item;
            }

            item.additionalAmount = decAdditionalAmount;

            #endregion

            return item;

        }
    }
}