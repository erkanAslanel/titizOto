using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Checkout
{
    public class checkoutPageItem
    {
        public string url { get; set; }
        public string name { get; set; }
        public ViewModel.Checkout.checkoutStep step { get; set; }
    }
}