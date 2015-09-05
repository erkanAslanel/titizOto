using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HelperAdmin;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_galleryMeta))]
    public partial class tbl_gallery
    {
        public string classTitle { get { return "Ürün Resim"; } }

        public static string getClassTitle() { return "Ürün Resim"; }

        public Dictionary<int, string> critearIdList(int productId)
        {
            var list = new Dictionary<int, string>();

            DbWithBasicFunction dbc = new DbWithBasicFunction();
            var db = dbc.db;

       


            var dbList = db.tbl_productCritear.Where(a => a.productId == productId).ToList();

            foreach (var item in dbList)
            {
                list.Add(item.tbl_critear.critearId, item.tbl_critear.name);
            }

            return list;
        }

        public string optionIdText
        {
            get
            {
                List<string> critearNameList = new List<string>();

                if (!string.IsNullOrWhiteSpace(optionList))
                {
                    DbWithBasicFunction dbc = new DbWithBasicFunction();

                    var list = optionList.Split(',');

                    int critearId = 0;

                    foreach (var item in list)
                    {
                        if (int.TryParse(item, out critearId))
                        {
                            var critearItem = dbc.db.tbl_critear.Where(a => a.critearId == critearId).FirstOrDefault();

                            if (critearItem != null)
                            {
                                critearNameList.Add(critearItem.name);
                            }
                        }
                    }

                    return string.Join(" | ", critearNameList);
                }
                else
                {
                    return "Seçenek Yok";
                }

            }
        }
    }


    public class tbl_galleryMeta
    {
        [DataType("primaryKey")]
        public int galleryId { get; set; }

        [DataType("primaryKey")]
        public int productId { get; set; }

        [Display(Name = "Fotoğraf")]
        [DataType("photoCut")]
        [Required]
        public string photo { get; set; }
        public string photoCoordinate { get; set; }

        [Display(Name = "Sırası")]
        public int sequence { get; set; }

        [Display(Name = "Durum")]
        [DataType("statu")]
        [Required]
        public bool statu { get; set; }

        [Display(Name = "Fotoğraf Code")]
        [DataType("primaryKey")]
        public string guid { get; set; }

     

        [Display(Name = "Ürün Seçeneği")]
        [DataType("dropDown")]
        public string optionList { get; set; }

    }
}