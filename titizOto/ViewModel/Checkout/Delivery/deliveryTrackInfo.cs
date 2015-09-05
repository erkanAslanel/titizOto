using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace ViewModel.Checkout.Delivery
{
    public class deliveryTrackInfo
    {
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formNameRequired")]
        [Display(ResourceType = typeof(lang), Name = "formName")]
        [DataType("normalText")]
        public string name { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formSurnameRequired")]
        [Display(ResourceType = typeof(lang), Name = "formSurname")]
        [DataType("normalText")]
        public string surname { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        [Display(ResourceType = typeof(lang), Name = "formEmail")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        [DataType("normalText")]
        public string email { get; set; }
    }
}