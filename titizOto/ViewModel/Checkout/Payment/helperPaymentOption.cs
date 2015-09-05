using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Checkout;

namespace ViewModel.Checkout.Payment
{
    public class helperPaymentOption : helperCheckoutShared
    {
        public int paymentOptionId { get; set; }
        public string transferDiscountText { get; set; }

        public bool isTransferVisiable { get; set; }
        public bool isCredicardVisiable { get; set; } 

    }

    public enum paymentOption
    {
        noAnswer = 0,
        transfer = 1,
        creditCard = 2
    }
}