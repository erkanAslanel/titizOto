using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.App_GlobalResources;
using ViewModel.Account.Address;
using ViewModel.Shared;

namespace ViewModel.Checkout.Delivery
{
    public class helperRegisterDelivery : helperCheckoutShared
    {
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutDeliveryRequired")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutDeliveryRequired")]
        public int selectedDeliveryAddressId { get; set; }

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

        [Display(ResourceType = typeof(lang), Name = "checkoutDeliverySameAddress")]
        public bool isSameBillingAddress { get; set; }
        public List<addressItem> addressList { get; set; }
        public string userguid { get; set; }
    }
}