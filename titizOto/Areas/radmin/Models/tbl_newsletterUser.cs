using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_newsletterUserMeta))]
    public partial class tbl_newsletterUser
    {

        public string classTitle { get { return "E-Bülten Aboneliği"; } }

        public static string getClassTitle() { return "E-Bülten Aboneliği"; }

    }

    public class tbl_newsletterUserMeta
    {
        [DataType("primaryKey")]
        public int newsletterUserId { get; set; }

        [Display(Name = "Email")]
        [DataType("normalText")]
        [Required(ErrorMessageResourceType = typeof(lang),
ErrorMessageResourceName = "emailRequired")]
        public string email { get; set; }

        [Display(Name = "İp No")]
        [DataType("normalText")]
        public string ipNo { get; set; }

        [Display(Name = "Eklenme Tarihi")]
        public System.DateTime createTime { get; set; }

         
    }
}