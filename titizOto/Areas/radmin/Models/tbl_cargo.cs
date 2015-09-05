using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.HelperAdmin.CustomBinder;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_cargoMeta))]
    [ModelBinder(typeof(cargoBinder))]
    public partial class tbl_cargo
    { 
        public string classTitle { get { return "Kargo Firması"; } }

        public static string getClassTitle() { return "Kargo Firması"; }
    }

    public class tbl_cargoMeta
    {
        [DataType("primaryKey")]
        public int cargoId { get; set; }

        [Display(Name = "Fiyat")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [DataType("normalText")]
        [Required]
        public decimal price { get; set; }

        [Display(Name = "Bedava Kargo Sepet Tutarı")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [DataType("normalText")]
        [Required]
        public decimal freeCargoPrice { get; set; }

        [Display(Name = "Fotoğraf")]
        [DataType("photoCut")]
        [Required]
        public string photo { get; set; }
        public string photoCoordinate { get; set; }

        [Display(Name = "Firma Adı")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Detay Açıklama")]
        [DataType("htmlContent")]
        [AllowHtml]
        public string detail { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }

        [Display(Name = "Kargo Alıcıya Ait")]
        [DataType("statu")]
        [Required]
        public bool isCargoPriceOnCustomer { get; set; }

        [Display(Name = "Kargo Takip Adresi")]
        [DataType("normalText")]
        public string trackUrl { get; set; }

    }
}