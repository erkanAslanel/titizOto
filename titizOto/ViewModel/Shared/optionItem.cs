using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Shared
{
    public struct optionItem
    {
        public string header { get; set; }
        public string headerId { get; set; }
        public  List<KeyValuePair<string, string>> options { get; set; }
    }
}