//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace titizOto.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_category
    {
        public tbl_category()
        {
            this.tbl_page = new HashSet<tbl_page>();
        }
    
        public int categoryId { get; set; }
        public int langId { get; set; }
        public string name { get; set; }
        public int parentId { get; set; }
        public int sequence { get; set; }
        public bool statu { get; set; }
        public bool isMainMenuShown { get; set; }
        public bool isFooterMenuShown { get; set; }
    
        public virtual ICollection<tbl_page> tbl_page { get; set; }
    }
}
