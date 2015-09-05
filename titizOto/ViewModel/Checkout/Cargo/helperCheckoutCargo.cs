using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace ViewModel.Checkout.Cargo
{
    public class helperCheckoutCargo : helperCheckoutShared
    {
        public List<cargoItem> cargoList { get; set; }  
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutCargoRequired")]
        public int selectedCargoId { get; set; }
    }
}