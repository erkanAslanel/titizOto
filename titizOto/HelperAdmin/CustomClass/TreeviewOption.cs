using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelperAdmin
{
    public class TreeviewOption
    { 
        public HtmlString jsonData { get; set; }
        public SelectionMode selectMode { get; set; }
        public string fieldName { get; set; }

        public TreeviewOption(HtmlString jsonData, SelectionMode selectMode, string fieldName)
        {
            this.jsonData = jsonData;
            this.selectMode = selectMode;
            this.fieldName = fieldName; 
        }

        public TreeviewOption()
        {
           
        }
    }

    public enum SelectionMode
    {
        Single = 1, Multi = 2
    }
}