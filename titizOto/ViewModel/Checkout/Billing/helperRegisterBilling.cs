using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.App_GlobalResources;
using ViewModel.Account.Address;

namespace ViewModel.Checkout.Billing
{
    public class helperRegisterBilling : helperCheckoutShared
    {
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutBillingRequired")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutBillingRequired")]
        public int selectedBillingAddressId { get; set; }

        public List<SelectListItem> getAddressDropDown()
        {
            var itemList = new List<SelectListItem>();

            if (addressList != null && addressList.Count > 0)
            {
                itemList = addressList.Select(a => new SelectListItem
                {
                    Value = a.addressId.ToString(),

                    Text = a.name + "(" + a.type + ")"
                    //Or whatever you want to display
                }).ToList();




            }

            var pleaseOption = new SelectListItem();
            pleaseOption.Text = lang.pleaseSelectAddress;
            pleaseOption.Value = "0";


            itemList.Add(pleaseOption);

            return itemList;
        }

       
        public List<addressItem> addressList { get; set; }
        public string userguid { get; set; }

    }
}