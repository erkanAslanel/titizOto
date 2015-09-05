using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.Page
{
    public class helperStaticPage : titleDescription
    {
        public List<leftMenuItem> leftMenuList { get; set; } 
        public string pageTitle { get; set; } 
        public string detail { get; set; } 
        public breadCrumb breadCrumbItem { get; set; }
    }
}