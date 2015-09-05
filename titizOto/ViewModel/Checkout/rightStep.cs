using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Checkout
{
    public class rightStep
    {
        public List<checkoutPageItem> stepLinkList { get; set; }
        public checkoutStep currentStep { get; set; }

        public rightStep(List<checkoutPageItem> stepLinkList, checkoutStep currentStep)
        {
            this.stepLinkList = stepLinkList;
            this.currentStep = currentStep; 
        }
    }
}