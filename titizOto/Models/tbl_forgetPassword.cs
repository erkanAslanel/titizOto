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
    
    public partial class tbl_forgetPassword
    {
        public int forgetPasswordId { get; set; }
        public int userId { get; set; }
        public string code { get; set; }
        public System.DateTime createTime { get; set; }
    }
}
