using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace ViewModel.Search
{
    public class searchKey
    {
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "searchRequired")]
        [StringLength(20, MinimumLength = 3, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "searchRequired")]
        public string keyWord { get; set; }

        public string searchLink { get; set; }
    }
}