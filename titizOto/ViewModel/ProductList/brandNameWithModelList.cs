using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.ProductList
{
    public struct brandNameWithModelList
    {
        public string brandName { get; set; }

        public string brandUrl { get; set; }


        public List<modelList> modelList { get; set; }
         
    }
}