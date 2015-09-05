using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace ViewModel.Checkout.Payment
{
    public class cardOptionItem
    {
        public int bankPosOptionId { get; set; }
        public string monthStr { get; set; }
        public decimal monthPrice { get; set; }
        public decimal totalPrice { get; set; }
        public string monthPriceStr { get; set; }
        public string totalPriceStr { get; set; }

    }
}