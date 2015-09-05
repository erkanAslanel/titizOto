using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_emailMeta))]
    public partial class tbl_email
    {
        public string classTitle { get { return "Kullanıcı Mailleri"; } }

        public static string getClassTitle() { return "Kullanıcı Mailleri"; }

        public Dictionary<int, string> emailTypeIdList()
        {
            var list = new Dictionary<int, string>();

            list.Add(0, "Lütfen Seçiniz");
            list.Add(1, "Aktivasyon");
            list.Add(2, "Teşekkür Emaili");
            list.Add(3, "Şifre Sıfırlama");
            list.Add(4, "Sipariş Tamamlandı Havale");
            list.Add(5, "Minimum Stok Bildirimi");
            list.Add(6, "Sipariş Durum Güncellemesi");
            list.Add(7, "Kargoya Verildi");

            return list;
        }

        public string emailTypeName
        {
            get
            {

                var itemList = emailTypeIdList().Where(a => a.Key == this.emailTypeId).ToList();

                if (itemList.Count > 0)
                {
                    return itemList.First().Value;
                }
                else
                {
                    return "Email Tipi Bulunamadı";
                }

            }
        }
    }

    public class tbl_emailMeta
    {
        [DataType("primaryKey")]
        public int emailId { get; set; }

        [Display(Name = "Email Tipi")]
        [DataType("dropDown")]
        [Range(1, 10, ErrorMessage = "Email tipini seçiniz.")]
        public int emailTypeId { get; set; }

        [Display(Name = "Dil")]
        [DataType("lang")]
        [Required]
        public int langId { get; set; }

        [Display(Name = "Mail Açıklaması")]
        [DataType("normalText")]
        public string description { get; set; }

        [Display(Name = "İçerik")]
        [DataType("htmlContent")]
        [AllowHtml]
        public string detail { get; set; }


        [Display(Name = "Mail Konusu")]
        [DataType("normalText")]
        [Required]
        public string title { get; set; }
    }
}