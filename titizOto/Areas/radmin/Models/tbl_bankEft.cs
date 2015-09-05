using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HelperAdmin;
using titizOto.App_GlobalResources;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_bankEftMeta))]
    public partial class tbl_bankEft
    {

        public string classTitle { get { return "Banka Havale"; } }
        public static string getClassTitle() { return "Banka Havale"; }

        public Dictionary<int, string> bankIdList()
        {

            DbWithBasicFunction dbc = new DbWithBasicFunction();
            var db = dbc.db;

            var list = new Dictionary<int, string>();

            var dataList = db.tbl_bank.ToList();

            foreach (var item in dataList)
            {
                list.Add(item.bankId, item.name);
            }



            return list;
        }
    }

    public class tbl_bankEftMeta
    {
        [DataType("primaryKey")] 
        [Display(ResourceType = typeof(lang), Name = "eftId")]
        
        public int bankEftId { get; set; } 

        [Display(ResourceType = typeof(lang), Name = "eftName")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; } 

        [Display(Name = "Banka")]
        [DataType("dropDown")]
        [Required]
        public int bankId { get; set; }

        [Display(ResourceType = typeof(lang), Name = "eftBranchCode")]
        [DataType("normalText")]
        [Required]
        public string branchCode { get; set; } 

        [Display(ResourceType = typeof(lang), Name = "eftAccountNo")]
        [DataType("normalText")]
        [Required]
        public string accountNo { get; set; }

        [Display(ResourceType = typeof(lang), Name = "eftIban")]
        [DataType("normalText")]
        [Required]
        public string iban { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }
    }
}