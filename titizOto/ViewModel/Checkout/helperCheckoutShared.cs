using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.Checkout
{
    public class helperCheckoutShared : titleDescription
    {
        public List<checkoutPageItem> stepLinkList { get; set; }
        public checkoutStep activeStep { get; set; }
        public string detail { get; set; } 
        public bool isMessageExist { get; set; }
        public string message { get; set; } 
    }
}