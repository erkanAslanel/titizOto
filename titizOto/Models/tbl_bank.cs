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
    
    public partial class tbl_bank
    {
        public tbl_bank()
        {
            this.tbl_bankEft = new HashSet<tbl_bankEft>();
            this.tbl_bankPos = new HashSet<tbl_bankPos>();
        }
    
        public int bankId { get; set; }
        public string name { get; set; }
        public string color1 { get; set; }
        public string color2 { get; set; }
        public string color3 { get; set; }
        public string logo { get; set; }
        public bool statu { get; set; }
        public int sequence { get; set; }
        public string paymentLogo { get; set; }
        public Nullable<int> langId { get; set; }
    
        public virtual ICollection<tbl_bankEft> tbl_bankEft { get; set; }
        public virtual ICollection<tbl_bankPos> tbl_bankPos { get; set; }
    }
}