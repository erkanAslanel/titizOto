using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using ViewModel.Shared;

namespace ViewModel.ProductDetail
{
    public class helperProductDetail: titleDescription
    {
        public breadCrumb breadCrumbItem { get; set; }
        public string backProductUrl { get; set; }
        public string nextProductUrl { get; set; }
        public bool isBackProductUrlExist { get; set; }
        public bool isNextProductUrlExist { get; set; }
        public string productName { get; set; }
        public string price { get; set; }
        public string detail { get; set; }
        public List<Tuple<string,string>>  imageList { get; set; }
        public string backLink { get; set; }
        public List<optionItem> optionList { get; set; }
        public int productId { get; set; }
        public bool isOptionMsgExist { get; set; }
        public bool isInstallmenTableVisible { get; set; }
        public string withoutTaxPrice { get; set; }
    }
}