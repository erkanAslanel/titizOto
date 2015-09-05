using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.ProductDetail
{
    public class payOptionItem
    {
        public string logoColor { get; set; }
        public string headerColor { get; set; }
        public string rowColor { get; set; }
        public string logoImg { get; set; }  
        public List<installmentItem> itemList { get; set; }
        public List<installmentItem> excluedeList { get; set; }
        public int containerId { get; set; }  

    }
}