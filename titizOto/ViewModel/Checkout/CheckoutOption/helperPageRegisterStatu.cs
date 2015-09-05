using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;
using ViewModel.Shared;

namespace ViewModel.Checkout.CheckoutOption
{
    public class helperPageRegisterStatu : helperCheckoutShared
    {

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutRegisterStatuRequired")]
        [Range(1, 2, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutRegisterStatuRequired")]
        public int registerStatu { get; set; }
      

    }
}