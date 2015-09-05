using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HelperAdmin;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_stockMeta))]
    public partial class tbl_stock
    {
        public string classTitle { get { return "Stok"; } }

        public static string getClassTitle() { return "Stok"; }

        public string getProductName()
        {
            DbWithBasicFunction dbc = new DbWithBasicFunction();
            var productItem = dbc.db.tbl_product.Where(a => a.productId == this.productId).FirstOrDefault();

            if (productItem != null)
            {
                return productItem.name;
            }
            else
            {
                return "";
            }
        }

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

    public class tbl_stockMeta
    {
        [DataType("primaryKey")]
        public int stockId { get; set; }

        [Display(Name = "Stok Adet")]
        [DataType("normalText")]
        [Required]
        [Range(1, int.MaxValue)]
        public int stockCount { get; set; }

        

        [Display(Name = "Ürün Seçeneği")]
        [DataType("dropDown")]
        public string optionList { get; set; }

        [Display(Name = "Minimum Uyarı Stok Adet")]
        [DataType("normalText")]
        [Required]
        [Range(0, int.MaxValue)]
        public int minCount { get; set; }

    }
}