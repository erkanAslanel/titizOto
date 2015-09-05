using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelperAdmin
{
    public class HierarchyData
    {
        public string title { get; set; }
        public string key { get; set; }
        public List<HierarchyData> children { get; set; }
        public bool expand { get; set; }
        public bool select { get; set; }
        public string addClass { get; set; }

        public HierarchyData(string label, int key, List<HierarchyData> children, bool isSelect, string addClass)
        {
            this.title = label;
            this.key = "category" + key.ToString();
            this.children = children;
            this.expand = true;
            this.select = isSelect;
            this.addClass = addClass;
        }

    }

}