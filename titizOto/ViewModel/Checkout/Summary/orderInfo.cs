using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Checkout.Summary
{
    public class orderInfo
    {
       
        public string customerNameSurname { get; set; } 
        public string customerPhone { get; set; }
        public string customerEmail { get; set; }
        public string customerBasket { get; set; }
        public string orderDate { get; set; }
        public string billingHtml { get; set; }
        public string deliveryHtml { get; set; }
        public string transferAccountHtml { get; set; }
    }
}