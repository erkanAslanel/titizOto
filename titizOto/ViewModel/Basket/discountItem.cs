using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Basket
{
    public class discountItem
    {
        public string name { get; set; }
        public string description { get; set; }
        public decimal discountAmount { get; set; }
        public int discountId { get; set; }
    }
}