using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Shared
{
    public struct footerMenu
    {
        public string url { get; set; }
        public string name { get; set; }
        public string hiddenUrl { get; set; }

        public List<footerMenu> children { get; set; }
    }
}