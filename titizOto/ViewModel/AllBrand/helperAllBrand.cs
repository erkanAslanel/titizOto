using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.AllBrand
{
    public class helperAllBrand : titleDescription
    {
        public string brandName { get; set; }

        public List<allBrand> brandList { get; set; }

        public breadCrumb breadCrumbItem { get; set; }
    }
}