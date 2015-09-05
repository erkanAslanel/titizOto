using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.Account.Dashboard
{
    public class helperDashboard : titleDescription
    { 
        public breadCrumb breadCrumbItem { get; set; }
        public List<leftMenuItem> leftMenuList { get; set; } 
        public string detail { get; set; } 
        public string nameSurname { get; set; }
        public string email { get; set; } 
        public bool isNewsletterRegister { get; set; }
        public string registerInfoLink { get; set; }
        public string registerAddressLink { get; set; }
        public string registerOrderLink { get; set; } 
        public orderItem lastOrder { get; set; }
        public titizOto.Models.tbl_address lastAddressItem { get; set; }
    }
}