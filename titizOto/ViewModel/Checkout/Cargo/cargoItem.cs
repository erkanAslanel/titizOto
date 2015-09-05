using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Checkout.Cargo
{
    public class cargoItem
    {
        public string photo { get; set; }
        public int cargoId { get; set; }
        public string cargoDetail { get; set; }
        public decimal price { get; set; }
        public string priceString { get; set; }
        public bool  isCargoOnCustomer { get; set; }
        public string name { get; set; }
    }
}