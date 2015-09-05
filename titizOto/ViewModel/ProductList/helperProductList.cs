﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.ProductList
{
    public class helperProductList:titleDescription
    { 
        public breadCrumb breadCrumbItem { get; set; } 
        public List<productSmall> productList { get; set; }

        public string header { get; set; }
        public paging pagingItem { get; set; } 
    }
}