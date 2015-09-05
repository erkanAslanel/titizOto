using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace ViewModel.Account.Address
{
    public class addressModelItem
    {
        public tbl_address addressItem { get; set; } 
        public string message { get; set; }
        public bool isMessageExist  { get; set; } 
        public bool isSuccess { get; set; } 
        public string  guid { get; set; }
        public string closeUrl { get; set; }
    }
}