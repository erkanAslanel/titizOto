using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.Account.Order
{
    public class helperOrder : titleDescription
    {
        public breadCrumb breadCrumbItem { get; set; }
        public List<leftMenuItem> leftMenuList { get; set; }
        public string detail { get; set; } 
        public List<orderItem> orderList { get; set; }
      
    }
}