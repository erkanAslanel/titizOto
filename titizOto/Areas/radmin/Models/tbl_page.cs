using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_pageMeta))]
    public partial class tbl_page
    {
        public string classTitle { get { return "İçerik Sayfa"; } }

        public static string getClassTitle() { return "İçerik Sayfa"; }

        public Dictionary<int, string> pageTypeIdList()
        {
            var list = new Dictionary<int, string>();

            list.Add(0, "Normal İçerik");
            list.Add(1, "Yönlendirme");
            list.Add(2, "Tüm Modeller");
            list.Add(3, "Ürünler Listesi");
            list.Add(4, "Açılır İçerik (S.S.S)");
            list.Add(5, "Kayıt - Giriş");
            list.Add(6, "(Sistem) Aktivasyon");
            list.Add(7, "(Sistem) Yeniden Aktivasyon Gönderme");
            list.Add(8, "(Sistem) Sepet");
            list.Add(9, "(Sistem) Şifremi Unuttum");
            list.Add(10, "(Sistem) Şifre Sıfırlama");
            list.Add(12, "(Sistem) Hesabım");
            list.Add(11, "(Sistem) Hesabım Genel Bakış");
            list.Add(13, "(Sistem) Hesabım Üyelik Bilgileri");
            list.Add(14, "(Sistem) Hesabım Şifre Değiştirme");
            list.Add(15, "(Sistem) Hesabım Adreslerim");
            list.Add(16, "(Sistem) Hesabım Siparişlerim");
            list.Add(28, "(Sistem) Hesabım Siparişlerim Detay");
            list.Add(29, "(Sistem) Hesabım Sipariş Sorgulama");
            list.Add(17, "(Sistem) Hesabım Kuponlarım");
            list.Add(18, "(Sistem) Arama");
            list.Add(25, "(Sistem)(Ödeme)Anasayfa");
            list.Add(19, "(Sistem)(Ödeme)KayıtDurumu");
            list.Add(20, "(Sistem)(Ödeme)Teslimat");
            list.Add(21, "(Sistem)(Ödeme)Fatura");
            list.Add(22, "(Sistem)(Ödeme)Kargo");
            list.Add(23, "(Sistem)(Ödeme)Ödeme");
            list.Add(24, "(Sistem)(Ödeme)Özet");
            list.Add(26, "(Sistem)(Ödeme)Tamamlandı");
            list.Add(27, "(Sistem)(Ödeme)Önemli Hata"); 

            return list;
        }

    }
    public class tbl_pageMeta
    {
        [DataType("primaryKey")]
        public int pageId { get; set; }

        [Display(Name = "Sayfa Adı")]
        [DataType("urlName")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Sayfa Adresi")]
        [DataType("url")]
        [Required]
        public string url { get; set; }

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }

        [Display(Name = "Sayfa Başlık (Opsiyonel)")]
        [DataType("normalText")]
        public string title { get; set; }

        [Display(Name = "Seo Anahtar Kelime")]
        [DataType("seoKeyword")]
        public string metaKeyword { get; set; }

        [Display(Name = "Seo Sayfa Açıklama")]
        [DataType("seoDescription")]
        public string metaDescription { get; set; }

        public int categoryId { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Sayfa Tipi")]
        [DataType("dropDown")]
        public int pageTypeId { get; set; }

        [Display(Name = "Yönlendirme Adresi (Opsiyonel)")]
        [DataType("normalText")]
        public string redirectPageUrl { get; set; }

        [Display(Name = "İçerik")]
        [DataType("htmlContent")]
        [AllowHtml]
        public string detail { get; set; }

        [Display(Name = "Geçiş Sayfası")]
        [DataType("statu")]
        [Required]
        public bool isHelperUrl { get; set; }

    }
}