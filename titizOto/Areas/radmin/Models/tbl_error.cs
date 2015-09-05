using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_errorMeta))]
    public partial class tbl_error
    {
        public string classTitle { get { return "Hata"; } }

        public static string getClassTitle() { return "Hata"; }
    }

    public class tbl_errorMeta
    {
        public int errorId { get; set; }

        [Display(Name = "Hata Yazısı")]
        [Required]
        [DataType("bigText")]
        [AllowHtml]
        public string errorText { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        [DataType("normalText")]
        [Required]
        public System.DateTime saveDate { get; set; }


    }
}