using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HelperAdmin;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_carModelMeta))]
    public partial class tbl_carModel
    {
        public string classTitle { get { return "Araç Model"; } }

        public static string getClassTitle() { return "Araç Model"; }

        public Dictionary<int, string> carBrandList(int langId)
        {
            var list = new Dictionary<int, string>();

            DbWithBasicFunction dbc = new DbWithBasicFunction();
            var db = dbc.db;

            int langIdGenerated = 1;

            if (langId != 0)
            {
                langIdGenerated = langId;
            }

            var brandList = db.tbl_carBrand.Where(a => a.langId == langIdGenerated).AsEnumerable();

            foreach (var item in brandList)
            {
                list.Add(item.carBrandId, item.name);
            }


            return list;
        }


    }

    public class tbl_carModelMeta
    {
        [DataType("primaryKey")]
        public int carModelId { get; set; }

        [Display(Name = "Model Adı")]
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

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }

        [Display(Name = "Araç Markası")]
        public int carBrandId { get; set; }

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