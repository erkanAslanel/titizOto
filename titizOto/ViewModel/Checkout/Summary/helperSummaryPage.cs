using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;
using ViewModel.Basket;
using ViewModel.Checkout.Payment;
using ViewModel.Shared;

namespace ViewModel.Checkout.Summary
{
    public class helperSummaryPage : helperCheckoutShared
    {
        public helperSummaryPage()
        {
            this.orderSummary = new orderSummary();
        
        }

        public orderSummary orderSummary { get; set; }

        public string salesAgreement { get; set; }
        public string preSalesAgreement { get; set; }

        [Display(ResourceType = typeof(lang), Name = "orderNote")]
        public string orderNote { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "salesAgreementRequired")]
        public bool isAgreementChecked { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "preSalesAgreementRequired")]
        public bool isPreSalesAgreementChecked { get; set; }
    }
}