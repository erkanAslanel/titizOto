using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;
using ViewModel.Shared;

namespace ViewModel.Account.Password
{
    public class helperChangePassword : titleDescription
    {
        public breadCrumb breadCrumbItem { get; set; }
        public List<leftMenuItem> leftMenuList { get; set; }
        public string detail { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formPasswordRequired")]
        [Display(ResourceType = typeof(lang), Name = "formPassword")]
        [StringLength(16, MinimumLength = 6, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "passwordLength")]
        public string password { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formPassworRepRequired")]
        [Display(ResourceType = typeof(lang), Name = "formPassworRep")]
        [StringLength(16, MinimumLength = 6, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "passwordLength")]
        public string passwordRep { get; set; }

        public bool isMessageExist { get; set; }

        public string message { get; set; }

        public string cancelUrl { get; set; }
    }
}