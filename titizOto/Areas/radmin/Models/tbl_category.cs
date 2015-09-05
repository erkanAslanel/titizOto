using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_categoryMeta))]
    public partial class tbl_category
    {
        public string classTitle { get { return "Kategori"; } }

        public static string getClassTitle() { return "Kategori"; }

     
         
    }

    public class tbl_categoryMeta
    {
        [DataType("primaryKey")]
        public int categoryId { get; set; }

        [Display(Name = "Kategori Adı")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; } 

 
        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; } 

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }

        [Display(Name = "Ana Menüde Çıkması")]
        [DataType("statu")]
        [Required]
        public bool isMainMenuShown { get; set; }

        [Display(Name = "Sayfa Altında Çıkması")]
        [DataType("statu")]
        [Required]
        public bool isFooterMenuShown { get; set; }


        [Display(Name = "Sırası")]
        public int sequence { get; set; }
    }
}