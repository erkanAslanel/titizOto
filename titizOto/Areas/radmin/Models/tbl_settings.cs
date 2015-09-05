using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.HelperAdmin.CustomBinder;


namespace titizOto.Models
{
    [MetadataType(typeof(settingsMetaData))]
    [ModelBinder(typeof(settingsBinder))]
    public partial class tbl_settings
    {
        public static string getClassTitle()
        {
            return "Site Ayarları";
        }

        public Dictionary<int, string> transferDiscountTypeList()
        {
            var list = new Dictionary<int, string>();

            list.Add(1, "Yüzde");
            list.Add(2, "Tutar");
            return list;
        }
    }

    public class settingsMetaData
    {
        [Display(Name = "Alt Sayfaların Sayfa Başlığı")]
        public string allPageTitle { get; set; }

        [Display(Name = "Ana Sayfa Başlığı")]
        public string mainPageTitle { get; set; }


        [Display(Name = "Mail Kullanıcı Adı")]
        public string mailUserName { get; set; }

        [Display(Name = "Mail Şifre")]
        public string mailPassword { get; set; }

        [Display(Name = "Mail Port Numarası")]
        public string mailPort { get; set; }

        [Display(Name = "Mail Kullanıcısı Gönderi Adı")]
        public string mailSentName { get; set; }

        [Display(Name = "Mail Gönderi Adresi")]
        public string mailSentAddress { get; set; }

        [Display(Name = "Gelen Mail Adresi")]
        public string mailReceiverAddress { get; set; }

        [Display(Name = "Mail Server Adresi")]
        public string mailSmtpServer { get; set; }

        [Display(Name = "Mail SSL")]
        [DataType("statu")]
        public Nullable<bool> mailIsEnableSSL { get; set; }

        [Display(Name = "Site Açıklaması(Description)")]
        [MaxLength(4000)]
        [AllowHtml]
        public string metaDescription { get; set; }

        [Display(Name = "Meta Alanı Devam")]
        [MaxLength(4000)]
        [AllowHtml]
        public string metaDescriptionAdditional { get; set; }

        [Display(Name = "Seo Anahtar Kelime")]
        [DataType("seoKeyword")]
        public string metaKeyword { get; set; }

        [Display(Name = "Dil Seçimi")]
        public int langId { get; set; }

        [Display(Name = "Aktivasyonlu Üyelik")]
        [DataType("statu")]
        [Required]
        public bool registerIsActivationExist { get; set; }

        [Display(Name = "Teşekkür Mesajı")]
        [DataType("statu")]
        [Required]
        public bool registerIsThankMessageSend { get; set; }

        [Display(Name = "Havale İndirimi")]
        [DataType("statu")]
        [Required]
        public Nullable<bool> isTransferDiscount { get; set; }

        [Display(Name = "İndirim Tipi")]
        [DataType("dropDown")]
        [Required]
        public Nullable<int> transferDiscountType { get; set; }

        [Display(Name = "İndirim Yüzde / İndirim Tutar")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [DataType("normalText")]
        [Required]
        public Nullable<decimal> transferDiscountAmount { get; set; }

        [Display(Name = "Havale ile Ödeme Alımı")]
        [DataType("statu")]
        [Required]
        public Nullable<bool> isTransferEnable { get; set; }

        [Display(Name = "Kredi Kart ile Ödeme Alımı")]
        [DataType("statu")]
        [Required]
        public Nullable<bool> isCrediCardEnable { get; set; }


        [Display(Name = "(Test)Ödeme Test Hesabı")]
        [DataType("normalText")]
        public string testAccountEmail { get; set; }

        [Display(Name = "(Test) Ödeme Başarılı Kart")]
        [DataType("normalText")]
        public string testSuccessfulCard { get; set; }

        [Display(Name = "(Test)1 TL lik çekim Test Kartı")]
        [DataType("normalText")]
        public string testValidCard { get; set; }
    }
}