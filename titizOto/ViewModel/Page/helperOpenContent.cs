using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.Page
{
    public class helperOpenContent : titleDescription
    {
        public List<openContentItem> list { get; set; }
        public string pageTitle { get; set; } 
        public List<leftMenuItem> leftMenuList { get; set; }
        public breadCrumb breadCrumbItem { get; set; }

    }
}