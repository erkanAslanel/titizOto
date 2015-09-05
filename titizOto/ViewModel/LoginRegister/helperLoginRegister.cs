using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.LoginRegister
{
    public class helperLoginRegister : titleDescription
    {
        public registerItem register { get; set; }
        public loginItem login { get; set; }
        public breadCrumb breadCrumbItem { get; set; }

        public bool isFacebookError { get; set; }
        public string facebookErrorMessage { get; set; }

        public bool isLoginRequeredShown { get; set; }
        public string isLoginRequeredMessage { get; set; }

    }
}