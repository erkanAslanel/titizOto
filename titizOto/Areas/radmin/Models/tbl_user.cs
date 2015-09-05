using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.App_GlobalResources;
using titizOto.HelperAdmin.CustomBinder;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_userMeta))]
    [ModelBinder(typeof(userBinder))]
    public partial class tbl_user
    {
        public string classTitle { get { return "Üye"; } }

        public static string getClassTitle() { return "Üye"; }

        public bool isPasswordUpdate { get; set; }
     
        public string Md5Converter { get; set; }

        public Dictionary<int, string> userTypeIdList()
        {
            var list = new Dictionary<int, string>();

            list.Add(1, "Normal Üye");
            list.Add(2, "Facebook");
            list.Add(3, "Üye Olmadan Satın Alım");

            return list;
        }

        public Dictionary<int, string> registerStatuList()
        {
            var list = new Dictionary<int, string>();
           
            list.Add(1, "Tam Üye");
            list.Add(2, "Aktivasyonu Beklenen"); 
            list.Add(3, "Engellenen Üye");

            return list;
        }
    }

    public class tbl_userMeta
    {
        [DataType("primaryKey")]
        public int userId { get; set; }

        [Display(Name = "Ad")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Soyad")]
        [DataType("normalText")]
        [Required]
        public string surname { get; set; }

        [Display(Name = "Email")]
        [DataType("normalText")]
        [Required]
        public string email { get; set; }

        [Display(Name = "Şifre")]
        [DataType("primaryKey")]
        public string password { get; set; }



        [Display(Name = "Doğum Tarihi")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public Nullable<System.DateTime> birthday { get; set; }

        [Display(Name = "Cinsiyet")]
        [DataType("normalText")]
        public Nullable<int> gender { get; set; }

        [Display(Name = "Üye Tipi")]
        [DataType("dropDown")]
        public int userTypeId { get; set; }

        [Display(Name = "Kayıt Durumu")]
        [DataType("dropDown")]
        public int registerStatuId { get; set; }

        [DataType("primaryKey")]
        public string guid { get; set; }

        [Display(Name = "Şifre Güncellemesi")]
        public bool isPasswordUpdate { get; set; }

     
        [Display(Name = "Şifre")]
        [DataType("normalText")]
        public string Md5Converter { get; set; }
    }

}