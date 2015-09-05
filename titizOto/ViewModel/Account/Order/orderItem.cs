using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Account.Order
{
    public class orderItem
    {
        public string orderNo { get; set; }
        public string orderGuid { get; set; }
        public string orderDateStr { get; set; }
        public string totalPriceStr { get; set; }
        public string orderStatu { get; set; }
        public string orderDetailLink { get; set; }
    }
}