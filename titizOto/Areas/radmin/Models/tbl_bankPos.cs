using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HelperAdmin;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_bankPosMeta))]
    public partial class tbl_bankPos
    {

        public string classTitle { get { return "Sanal Pos"; } }
        public static string getClassTitle() { return "Sanal Pos"; }

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

        public Dictionary<int, string> posTypeIdList()
        {
            var list = new Dictionary<int, string>();

            list.Add(1, "Standart Pos");  
            return list;
        }

        public Dictionary<int, string> posCodeList()
        {
            var list = new Dictionary<int, string>();

            list.Add(1, "garantiPos");
            return list;
        }
    }

    

    public class tbl_bankPosMeta
    {
        [DataType("primaryKey")]
        public int bankPosId { get; set; }

        [Display(Name = "Pos Adı")]
        [DataType("normalText")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Banka")]
        [DataType("dropDown")]
        [Required]
        public int bankId { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }
     
        [Display(Name = "Ana pos ( Harici Kartların Çekimi )")]
        [DataType("statu")]
        [Required]
        public bool isMainPos { get; set; }

        [Display(Name = "Pos Kategorisi")]
        [DataType("dropDown")]
        public int posTypeId { get; set; }


        [Display(Name = "Taksit tablosunda göster")]
        [DataType("statu")]
        [Required]
        public bool isPosShownOnTable { get; set; }

        [Display(Name = "Pos Code")]
        [DataType("dropDown")]
        public string posCode { get; set; }
    }
}