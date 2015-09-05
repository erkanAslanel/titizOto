using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace ViewModel.Checkout.Payment
{
    public class cardOption
    {
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutCardOptionRequired")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutCardOptionRequired")]
        public int creditOptionId { get; set; }

        public List<cardOptionItem> optionList { get; set; }

        public bool isErrorExist { get; set; }
    }
}