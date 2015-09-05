using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Checkout.Payment
{
    public class transferDiscount
    {
        public bool isDiscountExist { get; set; }
        public transfetDiscountType discountType { get; set; }
        public decimal amount { get; set; }

        public decimal calcDiscountAmount(decimal totalPrice)
        {

            switch (discountType)
            {
                case transfetDiscountType.percentage:
                   return totalPrice * (amount / 100);

                  
                case transfetDiscountType.amount:

                    var calcAmount = totalPrice - amount;
                    if (calcAmount>0)
                    {
                        return calcAmount;
                    }
                    else
                    {
                        return 0;
                    } 
            }

            return 0;
        
        }
    }

    public enum transfetDiscountType
    {
        percentage = 1,
        amount = 2
    }
}