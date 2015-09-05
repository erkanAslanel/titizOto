using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Shared
{
    public class topCart
    { 
        public bool isRegisteredUser { get; set; }
        public int productCount { get; set; }
        public string nameSurname { get; set; }
        public string guestGuid { get; set; }
        public int userId { get; set; }
        public string registerUrl { get; set; }
        public string basketUrl { get; set; }
        public string userGuid { get; set; }
        public string basketIdString { get; set; }
    }
}