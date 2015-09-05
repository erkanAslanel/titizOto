using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.Search
{
    public class helperSearch : titleDescription
    {
        public breadCrumb breadCrumbItem { get; set; }
        public string detail { get; set; }
        public string searchWord  { get; set; }
        public string resultFound { get; set; } 
        public string searchKeyWordList { get; set; }
        public string searchLink { get; set; }
        public paging pagingItem { get; set; } 
             
        public  List<searchResultItem> searchList { get; set; }
    }
}