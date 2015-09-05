using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Account.Dashboard
{
    public class orderItem
    {
        public string orderNo { get; set; }
        public string orderUpdateTime { get; set; }
        public string statu { get; set; }

        public string cargoNo { get; set; }
        public string cargoTrackUrl { get; set; }
    }
}