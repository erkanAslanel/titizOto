using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_addressMeta))]
    public partial class tbl_address
    { 
        public string classTitle { get { return "Kullanıcı Adres"; } } 

        public static string getClassTitle() { return "Kullanıcı Adres"; }
         
    }

    public class tbl_addressMeta
    {
        [DataType("primaryKey")]
        public int addressId { get; set; }

        public int userId { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        public bool statu { get; set; }

        //-------name-------------
        [Display(ResourceType = typeof(lang), Name = "addressName")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressNameRequired")]
        [DataType("normalText")]
        public string name { get; set; }

       
        //-------isPersonal-------------
        [Display(ResourceType = typeof(lang), Name = "addressType")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressTypeRequired")]
        public bool isPersonal { get; set; }


        //-------tcNo-------------
        [Display(ResourceType = typeof(lang), Name = "addressTcNo")]
        [DataType("normalText")]
        public string tcNo { get; set; }


        //-------city-------------
        [Display(ResourceType = typeof(lang), Name = "addressCity")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressCityRequired")]
        [DataType("normalText")]
        public string city { get; set; }


        //-------district-------------
        [Display(ResourceType = typeof(lang), Name = "addressDistrict")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressDistrictRequired")]
        [DataType("normalText")]
        public string district { get; set; }


        //-------postCode-------------
        [Display(ResourceType = typeof(lang), Name = "addressPostCode")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressPostCodeRequired")]
        [RegularExpression(@"^\d{5}$", ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressPostCodeRequired")]
        [DataType("normalText")]
        public string postCode { get; set; }


        //-------phone-------------
        [Display(ResourceType = typeof(lang), Name = "addressPhone")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressPhoneRequired")]
        [RegularExpression(@"^[(]{1}\d{3}[)]{1}\d{3}[-]{1}\d{2}[-]{1}\d{2}$", ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressPhoneRequired")]
        [DataType("normalText")]
        public string phone { get; set; }


        //-------address-------------
        [Display(ResourceType = typeof(lang), Name = "address")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressRequired")]
        [DataType("normalText")]
        public string address { get; set; }


        //-------companyName------------- 
        [Display(ResourceType = typeof(lang), Name = "addressCompanyName")]
        [DataType("normalText")]
        public string companyName { get; set; }


        //-------taxOffice------------- 
        [Display(ResourceType = typeof(lang), Name = "addressTaxOffice")]
        [DataType("normalText")]
        public string taxOffice { get; set; }


        //-------taxNo------------- 
        [Display(ResourceType = typeof(lang), Name = "addressTaxNo")]
        [DataType("normalText")]
        public string taxNo { get; set; }


        //-------deliverPerson-------------
        [Display(ResourceType = typeof(lang), Name = "addressDeliveredPerson")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "addressDeliveredPersonRequired")]
        [DataType("normalText")]
        public string deliverPerson { get; set; }


        [Display(Name = "Sırası")]
        public int sequence { get; set; }

    }
}