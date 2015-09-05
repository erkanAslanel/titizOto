using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Checkout;

namespace ViewModel.Checkout.Complete
{
    public class helperComplete : helperCheckoutShared
    {
        public bool isTransferOrder  { get; set; }  
        public string transferAcountHtml { get; set; }
    }
}