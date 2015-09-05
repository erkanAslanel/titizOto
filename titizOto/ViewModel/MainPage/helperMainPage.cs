using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.MainPage
{
    public class helperMainPage : titleDescription
    { 
        public List<productSmall> newProductList { get; set; } 
        public string brandUrl { get; set; }
        public string allBrandName { get; set; }
    }
}