using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_productCritearMeta))]
    public partial class tbl_productCritear
    { 
        public string classTitle { get { return "Ürüne Bağlı Seçenek"; } }

        public static string getClassTitle() { return "Ürüne Bağlı Seçenek"; }
    }

    public class tbl_productCritearMeta
    {
        [DataType("primaryKey")]
        public int productCritearId { get; set; }

        [DataType("primaryKey")]
        public int productId { get; set; }

         [Display(Name = "Sırası")]
        public int sequence { get; set; }
    
    }
}