using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_sliderMeta))]
    public partial class tbl_slider
    {
        public string classTitle { get { return "Slider"; } }

        public static string getClassTitle() { return "Slider"; }


    }

    public class tbl_sliderMeta
    {
        [DataType("primaryKey")]
        public int sliderId { get; set; }

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }

        [Display(Name = "Slider Adı")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Başlık Yazı(Opsiyone)")]
        [DataType("normalText")]
        public string title { get; set; }

        [Display(Name = "Alt Başlık Yazı(Opsiyone)")]
        [DataType("normalText")]
        public string subTitle { get; set; }

        [Display(Name = "Link Yönelendirmesi")]
        [DataType("statu")]
        [Required]
        public bool isUrlActive { get; set; }

        [Display(Name = "Link Adresi")]
        [DataType("normalText")]
        public string urlText { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }

        [DataType("uploader")]
        [Required]
        public string photo { get; set; }

    }
}