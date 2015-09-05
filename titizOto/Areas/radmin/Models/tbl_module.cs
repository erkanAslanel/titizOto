using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_moduleMeta))]
    public partial class tbl_module
    {
        public string classTitle { get { return "Modül"; } }

        public static string getClassTitle() { return "Modül"; }

        public Dictionary<int, string> typeIdList()
        {
            var list = new Dictionary<int, string>();

            list.Add(0, "Standart Modül");
            list.Add(1, "Mesafeli Satış Sözleşmesi");
            list.Add(2, "Ön Sipariş Formu");
            list.Add(3, "Üyelik Sözleşmesi");

            return list;
        }

    }

    public class tbl_moduleMeta
    {
        public int moduleId { get; set; }

        [Display(Name = "Modül Adı")]
        [Required]
        [DataType("normalText")]
        public string name { get; set; }

        [Display(Name = "İçerik")]
        [Required]
        [AllowHtml]
        [DataType("htmlContent")]
        public string htmlContent { get; set; }

        public string tag { get; set; }

        [Display(Name = "Modül Tipi")]
        [DataType("dropDown")]
        public int typeId { get; set; }

        [Display(Name = "Modül Açıklaması")]
        [DataType("seoDescription")]
        public string description { get; set; }


        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }
    }

}