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
    
    public partial class tbl_carBrand
    {
        public tbl_carBrand()
        {
            this.tbl_carModel = new HashSet<tbl_carModel>();
        }
    
        public int carBrandId { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool statu { get; set; }
        public int sequence { get; set; }
        public bool isManuelUrl { get; set; }
        public string photo { get; set; }
        public string photoCoordinate { get; set; }
        public int langId { get; set; }
        public bool isMainPageShown { get; set; }
        public string metaKeyword { get; set; }
        public string metaDescription { get; set; }
        public string title { get; set; }
    
        public virtual ICollection<tbl_carModel> tbl_carModel { get; set; }
    }
}
