using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;
using ViewModel.Shared;

namespace ViewModel.LoginRegister
{
    public class helperForgetPassword : titleDescription
    {
        public breadCrumb breadCrumbItem { get; set; }
        public string message { get; set; }
        public bool isMessageExist { get; set; }
        public string loginLink { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        [Display(ResourceType = typeof(lang), Name = "formEmail")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        public string email { get; set; }
    }
}