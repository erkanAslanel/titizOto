using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.HelperAdmin.CustomBinder;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_bankPosOptionMeta))]
    [ModelBinder(typeof(bankPosOptionBinder))]
    public partial class tbl_bankPosOption
    {

        public string classTitle { get { return "Taksit Seçeneği"; } }
        public static string getClassTitle() { return "Taksit Seçeneği"; }

    }

    public class tbl_bankPosOptionMeta
    {
        [DataType("primaryKey")]
        public int bankPosOptionId { get; set; }

        [Display(Name = "Taksit Sayısı")]
        [DataType("normalText")]
        [Required]
        [Range(2, 36)]
        public int paymentCount { get; set; }

        [Display(Name = "Minimum Sepet Tutarı")]
        [DataType("normalText")]
        [Required]
        [Range(0, double.MaxValue)]
        public decimal minBasketAmount { get; set; }

        [Display(Name = "Vade Farkı ( Yüzde )")]
        [DataType("normalText")]
        [Required]
        [Range(0, double.MaxValue)]
        public decimal additionalAmount { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }
         
        public int bankPosId { get; set; }

    }
}