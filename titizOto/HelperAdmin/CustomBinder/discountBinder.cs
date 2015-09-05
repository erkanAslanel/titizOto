using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace titizOto.HelperAdmin.CustomBinder
{
    public class discountBinder : System.Web.Mvc.DefaultModelBinder
    {
        public override object BindModel(System.Web.Mvc.ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            tbl_discount item = ((tbl_discount)base.BindModel(controllerContext, bindingContext));

            if (string.IsNullOrWhiteSpace(item.code))
            { 
                 return item;
            }

            item.code = item.code.Trim();

            HttpRequestBase request = controllerContext.HttpContext.Request;

            var enUsCulture = CultureInfo.CreateSpecificCulture("en-US");
            var trCulture = CultureInfo.CreateSpecificCulture("tr-TR");

            bindingContext.ModelState.Remove("amountPercent");
            bindingContext.ModelState.Remove("minBasketAmount");
            bindingContext.ModelState.Remove("startDate");
            bindingContext.ModelState.Remove("endDate"); 

            #region amount

            string amount = request.Form.Get("amountPercent");
            decimal decAmount = 0;
            if (!decimal.TryParse(amount, NumberStyles.AllowDecimalPoint, enUsCulture, out decAmount))
            {
                bindingContext.ModelState.AddModelError("amountPercent", "Yüzde / Tutar alanı uygun formatta değil. Örnek giriş 99.99");
                return item;
            }
            item.amountPercent = decAmount;

            #endregion

            #region minBasket

            string minBasketAmount = request.Form.Get("minBasketAmount");
            decimal decMinBasketAmount = 0;
            if (!decimal.TryParse(minBasketAmount, NumberStyles.AllowDecimalPoint, enUsCulture, out decMinBasketAmount))
            {
                bindingContext.ModelState.AddModelError("minBasketAmount", "Tutar alanı uygun formatta değil. Örnek giriş 99.99");
                return item;
            }

            item.minBasketAmount = decMinBasketAmount;

            #endregion

            #region startDate

            string strStartDate = request.Form.Get("startDate");
            DateTime startDate;

            if (!DateTime.TryParse(strStartDate, trCulture, DateTimeStyles.None, out startDate))
            {
                bindingContext.ModelState.AddModelError("startDate", "Tarih girişi uygun değil.");
                return item;
            }

            item.startDate = startDate;

            #endregion

            #region endDate

            string strEndDate = request.Form.Get("endDate");
            DateTime endDate;

            if (!DateTime.TryParse(strEndDate, trCulture, DateTimeStyles.None, out endDate))
            {
                bindingContext.ModelState.AddModelError("endDate", "Tarih girişi uygun değil.");
                return item;
            }

            item.endDate = endDate;

            #endregion

            #region product

            string strProductList = request.Form.Get("strProductList");
            if (string.IsNullOrWhiteSpace(strProductList))
            {
                item.productList = null;
            }
            else
            {
                item.productList = strProductList;
            }

            #endregion

            #region Exclude product

            string strExculudeProductList = request.Form.Get("strExculudeProductList");
            if (string.IsNullOrWhiteSpace(strExculudeProductList))
            {
                item.exculudeProductList = null;
            }
            else
            {
                item.exculudeProductList = strExculudeProductList;
            }

            #endregion  

            return item;
        }
    }
}