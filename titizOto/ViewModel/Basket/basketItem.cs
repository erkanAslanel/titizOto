using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Basket
{
    public class basketItem
    {
        public int basketId { get; set; }
        public string discountCode { get; set; }
        public string photo { get; set; }
        public string description { get; set; }
        public List<Tuple<string, string>> optionItemList { get; set; } 
        public decimal productPriceDec { get; set; }
        public int quantity { get; set; } 
        public decimal productTotalPriceDec { get; set; }
        public int productId { get; set; }
        public string optionCode { get; set; }

        public string productDescriptionWithOptionItem
        {
            get
            {
                if (optionItemList != null)
                {
                    string descriptionText = description + "<br />";

                    foreach (var item in optionItemList)
                    {
                        descriptionText = descriptionText + "<div class=\"basketOptionItem\"><div class=\"c1 bold\">" + item.Item1 + "</div> : <div class=\"c1\">" + item.Item2 + "</div></div>";
                    }

                    return descriptionText;

                }
                else
                {
                    return description;
                }
            }
        }
    }


}