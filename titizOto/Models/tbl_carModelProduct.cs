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
    
    public partial class tbl_carModelProduct
    {
        public int carModelProductId { get; set; }
        public int productId { get; set; }
        public int carModelId { get; set; }
        public int sequence { get; set; }
    
        public virtual tbl_carModel tbl_carModel { get; set; }
        public virtual tbl_product tbl_product { get; set; }
    }
}
