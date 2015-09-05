using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Shared
{
    public class productSmall
    {
        public string t1 { get { return name; } }
        public string shortDescription { get; set; } 
        public int productId { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string photo { get; set; }
        public string price { get; set; }
        public string currency { get; set; }
        public string width { get; set; }
        public string height { get; set; } 
        public string withoutTaxPrice { get; set; }
    }
}