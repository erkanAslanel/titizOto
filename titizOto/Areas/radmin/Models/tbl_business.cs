using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_businessMeta))]
    public partial class tbl_business
    {
       
        public string classTitle { get { return "Bayi"; } }
        public static string getClassTitle() { return "Bayi"; }

    }

    public class tbl_businessMeta
    {
        [DataType("primaryKey")]
        public int businessId { get; set; }

        [Display(Name = "Bayi Adı")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; }




    }
}