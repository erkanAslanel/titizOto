using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HelperSite.Shared;
using ViewModel.Shared;

namespace ViewModel.Basket
{
    public class helperBasket : titleDescription
    {
        public helperBasket()
        {

            this.priceStringFormat = new CultureInfo("en-US");
            basketList = new List<basketItem>();
            discountList = new List<discountItem>();
        }


        public List<basketItem> basketList { get; set; }
        public List<discountItem> discountList { get; set; }
        public string totalPriceStr { get; set; }
        public decimal totalPriceDec { get; set; }
        public decimal discountTotalAmount { get; set; }
        public string continueShopUrl { get; set; }
        public string checkoutUrl { get; set; }
        public string updateBasketUrl { get; set; }
        public string discountCodeString { get; set; }
        public basketActionResult actionMsg { get; set; }
        public bool isDiscountValid { get; set; }
        public bool isDiscountExist
        {
            get
            {
                if (discountList != null && discountList.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        public bool isBasketExist
        {
            get
            {

                if (basketList != null && basketList.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public void calculateSum()
        {
            decimal discountAmount = 0;

            if (discountList != null && discountList.Count > 0)
            {
                discountAmount = this.discountList.Sum(a => a.discountAmount);
                this.discountCodeString = string.Join(",", discountList.Select(a => a.name).ToList());
            }

            var totalProductPrice = this.basketList.Sum(a => a.productTotalPriceDec);

            this.totalPriceDec = totalProductPrice - discountAmount;

            if (this.totalPriceDec < 0)
            {
                this.totalPriceDec = 0;
            }

            this.discountTotalAmount = discountAmount = 0;
            this.totalPriceStr = totalPriceDec.ToString("F2", priceStringFormat);

        }
        public breadCrumb breadCrumbItem { get; set; }
        public bool isBasketValid
        {
            get
            {
                if (actionMsg == basketActionResult.redirect)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public discountErrorType discountError { get; set; }
        public CultureInfo priceStringFormat { get; set; }

        public bool isMessageExist { get; set; }
        public string message { get; set; }
    }

    public enum basketActionResult
    {
        stockAdjust = 1,
        success = 2,
        redirect = 3,
        error = 4
    }
}