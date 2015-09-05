using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_critearMeta))]
    public partial class tbl_critear
    {
        public string classTitle { get { return "Ürün Seçenek"; } }

        public static string getClassTitle() { return "Ürün Seçenek"; }

    }

    public class tbl_critearMeta
    {
        [DataType("primaryKey")]
        public int critearId { get; set; }

        [Display(Name = "Seçenek Adı")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; }

        [Required]
        public int parentId { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }


        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; } 

    }
}