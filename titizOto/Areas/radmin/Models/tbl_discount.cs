using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_discountMeta))]
    [ModelBinder(typeof(titizOto.HelperAdmin.CustomBinder.discountBinder))]
    public partial class tbl_discount
    {
        public string classTitle { get { return "İndirim Kuponu"; } }

        public static string getClassTitle() { return "İndirim Kuponu"; }

        public Dictionary<int, string> typeIdList()
        {
            var list = new Dictionary<int, string>();

            list.Add(1, "Genel Sepet Tutar İndirimi");
            list.Add(2, "Genel Sepet Yüzde İndirimi");
            list.Add(3, "Ürün Tutar İndirimi");
            list.Add(4, "Ürün Yüzde İndirimi");


            return list;
        }

        public Dictionary<int, string> userIdList()
        {

            var list = new Dictionary<int, string>();

            list.Add(0, "Kullanıcı Tanımlamasız");

            return list;
        }

        public Dictionary<string, string> productIdList()
        {
            var list = new Dictionary<string, string>();

            DbWithBasicFunction dbf = new DbWithBasicFunction();

            var productList = dbf.db.tbl_product.OrderBy(a => a.name).Select(a => new { a.productId, a.name }).ToList();

             
            foreach (var item in productList)
            {
                list.Add(item.productId.ToString(), item.name);
            }

            return list;

        }

        

    }
    public class tbl_discountMeta
    {
        [DataType("primaryKey")]
        public int discountId { get; set; }

        [Display(Name = "İndirim Code")]
        [DataType("normalText")]
        [Required]
        public string code { get; set; }

        [Display(Name = "Açıklama (Opsiyonel)")]
        [DataType("normalText")]
        public string description { get; set; }

        [Display(Name = "İndirim Tipi")]
        [DataType("dropDown")]
        public int typeId { get; set; }

        [Display(Name = "Tutar / Yüzde")]
        [DataType("normalText")]
        public decimal amountPercent { get; set; }

        [Display(Name = "Minimum Sepet Tutarı")]
        [DataType("normalText")]
        [Required]
        public decimal minBasketAmount { get; set; }

        [Display(Name = "Minimum Ürün Sayısı")]
        [DataType("normalText")]
        [Required]
        public int minBasketCount { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        [DataType("normalText")]
        public System.DateTime startDate { get; set; }


        [Display(Name = "Bitiş Tarihi")]
        [DataType("normalText")]
        public System.DateTime endDate { get; set; }

        [Display(Name = "Kullanıcı")]
        [DataType("dropDown")]
        public int userId { get; set; }

        [Display(Name = "Ürün Kısıtlaması")]
        public string productList { get; set; }

        [Display(Name = "Hariç Tutulacak Ürünler")]
        public string exculudeProductList { get; set; }

        [Display(Name = "Diğer Kupon İle Birleştirme")]
        [DataType("statu")]
        [Required]
        public bool isOtherCombine { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Tekrar Sayısı")]
        [DataType("normalText")]
        [Required]
        public int repTime { get; set; }

        public bool isManuelGenerated { get; set; }

    }

    public enum discountType
    {
        basketPercent = 2,
        basketAmount = 1,
        productPercent = 4,
        productAmount = 3,

    }
}