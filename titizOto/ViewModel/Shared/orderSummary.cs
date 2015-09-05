using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Basket;
using ViewModel.Checkout.Payment;

namespace ViewModel.Shared
{
    public class orderSummary
    {
        public orderSummary()
        {

            this.basketItem = new helperBasket();

        }


        public helperBasket basketItem { get; set; }
        public string cargoPriceStr { get; set; }
        public string additionalPriceStr { get; set; }

        public paymentOption paymentOptionChoose { get; set; }
        public string paymentOptionChooseStr { get; set; }

        public decimal cargoPrice { get; set; }
        public decimal productPrice { get; set; }
        public string productPriceStr { get; set; }
        public decimal additionalPrice { get; set; }
        public decimal allTotalPrice { get; set; }
        public string allTotalPriceStr { get; set; }




        public string parentClass { get; set; }
        public decimal productDiscountPrice { get; set; }
        public string productDiscountPriceStr { get; set; }
        public bool isDiscountExist { get; set; }

        public bool isTransferDiscountExist { get; set; }
        public decimal transferDiscount { get; set; }
        public string transferDiscountStr { get; set; }

        public string discountCodeString { get; set; }
        public bool isCargoOnCustomer { get; set; }

    }
}