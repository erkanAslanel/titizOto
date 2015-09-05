using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_carBrandMeta))]
    public partial class tbl_carBrand
    {
        public string classTitle { get { return "Araç Marka"; } }

        public static string getClassTitle() { return "Araç Marka"; }

    }

    public class tbl_carBrandMeta
    {

        [DataType("primaryKey")]
        public int carBrandId { get; set; }

        [Display(Name = "Marka Adı")]
        [DataType("urlName")]
        [Required]
        public string name { get; set; }


        [Display(Name = "Sayfa Adresi")]
        [DataType("url")]
        [Required]
        public string url { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }

        [DataType("photoCut")]
        [Required]
        public string photo { get; set; }
        public string photoCoordinate { get; set; }

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }

        [Display(Name = "Ana Sayfada Gösterim")]
        [DataType("statu")]
        [Required]
        public bool isMainPageShown { get; set; }

        [Display(Name = "Sayfa Başlık (Opsiyonel)")]
        [DataType("normalText")]
        public string title { get; set; }

        [Display(Name = "Seo Anahtar Kelime")]
        [DataType("seoKeyword")]
        public string metaKeyword { get; set; }

        [Display(Name = "Seo Sayfa Açıklama")]
        [DataType("seoDescription")]
        public string metaDescription { get; set; }

    }
}