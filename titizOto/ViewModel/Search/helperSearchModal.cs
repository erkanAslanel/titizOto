using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Search
{
    public class helperSearchModal
    {
        public List<searchResultItem> searchList { get; set; } 
        public int totalSearch { get; set; } 
        public string searchLink { get; set; }
    }
}