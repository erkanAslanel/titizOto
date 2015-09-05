using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.Account.Address
{
    public class helperAddress : titleDescription
    {
        public breadCrumb breadCrumbItem { get; set; }
        public List<leftMenuItem> leftMenuList { get; set; }
        public string detail { get; set; }
        public List<addressItem> addressList { get; set; }
        public string userguid { get; set; }
    }

}