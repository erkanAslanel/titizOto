using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;
using titizOto.HelperAdmin.CustomBinder;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_productMeta))]
    [ModelBinder(typeof(productBinder))]
    public partial class tbl_product
    {
        public string classTitle { get { return "Ürün"; } }

        public static string getClassTitle() { return "Ürün"; }

        public Dictionary<int, string> brandList()
        {
            var list = new Dictionary<int, string>();

            DbWithBasicFunction dbc = new DbWithBasicFunction();
            var db = dbc.db;

            list.Add(0, "Marka Seçili Değil");


            var dbList = db.tbl_brand.ToList();

            foreach (var item in dbList)
            {
                list.Add(item.brandId, item.name);
            }

            return list;
        }

        public Dictionary<int, string> businessList()
        {
            var list = new Dictionary<int, string>();

            DbWithBasicFunction dbc = new DbWithBasicFunction();
            var db = dbc.db;

            list.Add(0, "Bayi Seçili Değil");


            var dbList = db.tbl_business.ToList();

            foreach (var item in dbList)
            {
                list.Add(item.businessId, item.name);
            }

            return list;
        }
    }

    public class tbl_productMeta
    {
        [DataType("primaryKey")]
        public int productId { get; set; }

        [Display(Name = "Ürün Markası")]
        [DataType("dropDown")]
        public int brandId { get; set; }

        [Display(Name = "Ürün Bayisi")]
        [DataType("dropDown")]
        public int businessId { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }

        [Display(Name = "Fiyat")]
        [DisplayFormat(DataFormatString = "{0:0.00}")] 
        [DataType("normalText")]
        [Required]
        public decimal price { get; set; }

        [Display(Name = "Kdv Dahil")]
        [DataType("statu")]
        [Required]
        public bool isTaxInclude { get; set; }

        [Display(Name = "Kdv Oranı")]
        [DataType("normalText")]
        [Required]
        public int taxPercent { get; set; }

        [Display(Name = "İndirimli Fiyatı Göster")] 
        [DataType("statu")]
        [Required]
        public bool isDiscountPriceActive { get; set; }

        [Display(Name = "İndirimli Fiyat")] 
        [DataType("normalText")]
        public decimal discountPrice { get; set; }

        [Display(Name = "Kısa Açıklama")]
        [DataType("bigText")]
        public string shortDescription { get; set; }

        [Display(Name = "Detay Açıklama")]
        [DataType("htmlContent")]
        [AllowHtml]
        public string detail { get; set; }

        [Display(Name = "Seo Anahtar Kelime")]
        [DataType("seoKeyword")]
        public string metaKeyword { get; set; }

        [Display(Name = "Seo Sayfa Açıklama")]
        [DataType("seoDescription")]
        public string metaDescription { get; set; }

        [Display(Name = "Özel Sayfa İsimlendirme(Opsiyonel)")]
        [DataType("normalText")]
        public string title { get; set; }

        [Display(Name = "Ürün Adı")]
        [DataType("urlName")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Ürün Adresi")]
        [DataType("url")]
        [Required]
        public string url { get; set; }

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }


        [Display(Name = "Anasayfadan Gösterim")]
        [DataType("statu")]
        [Required]
        public bool isShowCase { get; set; }

    }
}