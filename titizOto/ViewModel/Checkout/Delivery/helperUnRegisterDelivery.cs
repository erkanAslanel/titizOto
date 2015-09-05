using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;
using ViewModel.Shared;

namespace ViewModel.Checkout.Delivery
{
    public class helperUnRegisterDelivery : helperCheckoutShared
    {
        public deliveryTrackInfo trackInfoItem { get; set; }


        [Display(ResourceType = typeof(lang), Name = "checkoutDeliverySameAddress")]
        public bool isSameBillingAddress { get; set; }

        public titizOto.Models.tbl_address addressItem { get; set; }

    }
}