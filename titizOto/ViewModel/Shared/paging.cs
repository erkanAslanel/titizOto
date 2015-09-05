using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Shared
{
    public struct paging
    { 
        public bool isPagingExist { get; set; }
        public int totalItems { get; set; }
        public int currentPage { get; set; }
        public int itemsPerPage { get; set; }
        public string pageSufix { get; set; }
    }
}