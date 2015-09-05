using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_adminUserMeta))]
    public partial class tbl_adminUser
    {
        public string classTitle { get { return "Panel Kullanıcıları"; } }

        public static string getClassTitle() { return "Panel Kullanıcıları"; }

        public Dictionary<int, string> roleList()
        {
            var list = new Dictionary<int, string>();
            list.Add(1, "Süper Admin");
            list.Add(2, "Genel Admin");
            return list;
        }

    }


    public class tbl_adminUserMeta
    {


        [DataType("primaryKey")]
        public int userId { get; set; }

        [Display(Name = "Mail")]
        [DataType("normalText")]
        [Required]
        public string email { get; set; }

        [Display(Name = "Şifre")]
        [DataType("normalText")]
        [Required]
        public string password { get; set; }

        [Display(Name = "Son Giriş Tarihi")]
        [DataType("normalText")]
        [Required]
        public System.DateTime enterDate { get; set; }


        [Display(Name = "Rol")]
        public int adminRoleId { get; set; }

        [Display(Name = "Ad")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; }


        [Display(Name = "Soyad")]
        [DataType("normalText")]
        [Required]
        public string surname { get; set; }
    }
}