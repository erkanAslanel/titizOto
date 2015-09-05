using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.Account.Order
{
    public class helperOrderAdminDetail
    {
        public string detail { get; set; }
        public orderSummary orderSummary { get; set; }
         
        public string orderNo { get; set; }

        public string orderStatuHtml { get; set; }

        public string deliveryAddress { get; set; }

        public string billingAddress { get; set; }

        public string orderNote { get; set; }

        public bool isRegistered { get; set; }

        public string nameSurname { get; set; } 

        public string email { get; set; }

        public string salesAgreement { get; set; }
        public string preSalesAgreement { get; set; }

    }
}