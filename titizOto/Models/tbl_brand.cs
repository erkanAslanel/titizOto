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
    
    public partial class tbl_brand
    {
        public tbl_brand()
        {
            this.tbl_product = new HashSet<tbl_product>();
        }
    
        public int brandId { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<tbl_product> tbl_product { get; set; }
    }
}
