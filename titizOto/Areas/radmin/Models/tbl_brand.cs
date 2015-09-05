using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_brandMeta))]
    public partial class tbl_brand
    {
        public string classTitle { get { return "Ürün Marka"; } }
        public static string getClassTitle() { return "Ürün Marka"; }
    }

    public class tbl_brandMeta
    {
        [DataType("primaryKey")]
        public int brandId { get; set; }

        [Display(Name = "Ürün Marka Adı")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; }
    
    }
}