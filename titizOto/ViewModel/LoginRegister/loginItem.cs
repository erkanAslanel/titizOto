using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace ViewModel.LoginRegister
{
    
    public class loginItem
    { 
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        [Display(ResourceType = typeof(lang), Name = "formEmail")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        public string email { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "loginItemLoginPasswordRequired")]
        [Display(ResourceType = typeof(lang), Name = "passwordName")]
        [StringLength(16, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "passwordLength", MinimumLength = 6)]
        public string password { get; set; } 

        public bool isRememberMe { get; set; } 

        public string forgetPasswordUrl { get; set; }

        public bool isMessageExist { get; set; }

        public string message { get; set; }
    }

   
}