using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;
using ViewModel.Shared;

namespace ViewModel.Account.Order
{
    public class helperOrderSearch : titleDescription
    {
        public breadCrumb breadCrumbItem { get; set; }
        public List<leftMenuItem> leftMenuList { get; set; }

        [Display(ResourceType = typeof(lang), Name = "orderListOrderNo")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "orderListOrderNoRequired")]
        [RegularExpression(@"^[0-9]{7}$", ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "orderNoSevenDigit")]
        [DataType("normalText")]
        public string orderNo { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        [Display(ResourceType = typeof(lang), Name = "formEmail")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        [DataType("normalText")]
        public string email { get; set; }

        public string toLogin { get; set; }
        public string detail { get; set; }

        public string message { get; set; }
        public bool isMessageExist { get; set; }
    }
}