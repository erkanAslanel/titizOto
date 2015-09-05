using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace ViewModel.Checkout.Payment
{
    public class transferInfo
    {
        public transferInfo()
        {
            this.transferDiscount = new transferDiscount();
            this.eftList = new List<titizOto.Models.tbl_bankEft>();
        }
  

        public transferDiscount transferDiscount { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "eftIdRequried")]
        public int selectedTransferId { get; set; }
        public List<titizOto.Models.tbl_bankEft> eftList { get; set; }

        public bool isMessageExist { get; set; }
        public string message { get; set; }
        
    }


}