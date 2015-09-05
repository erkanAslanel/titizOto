using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_bankMeta))]
    public partial class tbl_bank
    {
        public string classTitle { get { return "Banka"; } }
        public static string getClassTitle() { return "Banka"; }
    }

    public class tbl_bankMeta
    {
        [DataType("primaryKey")]
        public int bankId { get; set; }

        [Display(Name = "Adı")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Logo Rengi")]
        [DataType("normalText")]
        public string color1 { get; set; }

        [Display(Name = "Taksit Tablo Başlık Rengi")]
        [DataType("normalText")]
        public string color2 { get; set; }

        [Display(Name = "Taksit Tablo Satır Rengi")]
        [DataType("normalText")]
        public string color3 { get; set; }


        [DataType("uploader")]
        public string logo { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }

        [DataType("uploader")]
        public string paymentLogo { get; set; }

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }


    }
}