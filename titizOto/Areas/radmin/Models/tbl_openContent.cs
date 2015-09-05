using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_openContentMeta))]
    public partial class tbl_openContent
    {
        public string classTitle { get { return "Açılır İçerik Sayfa"; } }

        public static string getClassTitle() { return "Açılır İçerik Sayfa"; }
    }

    public class tbl_openContentMeta
    {
        [DataType("primaryKey")]
        public int openContentId { get; set; }

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }

        [Display(Name = "Başlık")]
        [DataType("normalText")]
        [MaxLength(4000)]
        public string title { get; set; }

        [Display(Name = "İçerik")]
        [DataType("htmlContent")]
        [AllowHtml]
        public string detail { get; set; }

    }
}