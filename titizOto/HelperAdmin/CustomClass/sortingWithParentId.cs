using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelperAdmin
{
    public class sortingWithParentId
    {

        public int depth { get; set; }
        public string item_id { get; set; }
        public int left { get; set; }
        public string parent_id { get; set; }
        public int right { get; set; }
    }
}