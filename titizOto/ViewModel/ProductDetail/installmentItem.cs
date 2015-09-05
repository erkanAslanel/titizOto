using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.ProductDetail
{
    public class installmentItem
    {
        public decimal insallmentAmount { get; set; }
        public decimal totalAmount { get; set; }
        public int payCount { get; set; }
        public bool isMinSpentRequired { get; set; }
        public decimal spentAmount { get; set; }

    }
}