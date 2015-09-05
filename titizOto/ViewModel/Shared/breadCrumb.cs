using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Shared
{
    public class breadCrumb
    { 
        public string name { get; set; }
        public string url { get; set; }

        public breadCrumb child { get; set; }
    }
}