using HelperAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.App_GlobalResources;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_orderMeta))]
    public partial class tbl_order
    {
        public string classTitle { get { return "Siparişler"; } }
        public static string getClassTitle() { return "Siparişler"; }

        public Dictionary<int, string> orderStatuList()
        {
            var list = new Dictionary<int, string>();

            list.Add(1, "Ödeme Bekliyor");
            list.Add(2, "Onaylandı");
            list.Add(3, "Hazırlanıyor");
            list.Add(4, "Tedarik Sürecinde");
            list.Add(5, "Kargoya Verildi");
            list.Add(6, "Teslim Edildi");
            list.Add(7, "İptal Edildi");


            return list;
        }

        public Dictionary<int, string> orderMailStatuList()
        {
            var list = new Dictionary<int, string>();

            list.Add(0, "Sipariş Tamamlandı Maili");
            list.Add(1, "Ödeme Bekliyor Maili");
            list.Add(2, "Onaylandı Maili");
            list.Add(3, "Hazırlanıyor Maili");
            list.Add(4, "Tedarik Sürecinde Maili");
            list.Add(5, "Kargoya Verildi Maili");
            list.Add(6, "Teslim Edildi Maili");
            list.Add(7, "İptal Edildi Maili");

            return list;
        }

        public string getOrderUserNameSurname()
        {
            DbWithBasicFunction dbc = new DbWithBasicFunction();

            if (this.isRegisteredOrder)
            {
                var userItem = dbc.db.tbl_user.Where(a => a.userId == this.userId).FirstOrDefault();

                if (userItem != null)
                {
                    return userItem.name + " " + userItem.surname;
                }
            }
            else
            {

                var trackInfo = dbc.db.tbl_trackInfo.Where(a => a.trackInfoId == this.trackInfoId).FirstOrDefault();

                if (trackInfo != null)
                {
                    return trackInfo.name + " " + trackInfo.surname;
                }

            }

            return "";
        }

        public string getOrderEmail()
        {
            DbWithBasicFunction dbc = new DbWithBasicFunction();

            if (this.isRegisteredOrder)
            {
                var userItem = dbc.db.tbl_user.Where(a => a.userId == this.userId).FirstOrDefault();

                if (userItem != null)
                {
                    return userItem.email;
                }
            }
            else
            {

                var trackInfo = dbc.db.tbl_trackInfo.Where(a => a.trackInfoId == this.trackInfoId).FirstOrDefault();

                if (trackInfo != null)
                {
                    return trackInfo.email;
                }

            }

            return "";
        }

        public string orderStatuStr
        {
            get
            {
                DbWithBasicFunction dbc = new DbWithBasicFunction();
                var os = new HelperSite.Shared.orderShared(dbc.db);
                return os.getOrderStatuString(this.orderStatu);

            }
        }

        public string orderTypeAndBank
        { 
            get
            {
                string val = "";

                switch (this.paymentTypeId)
                {
                    case 1:

                        val = "Havale";

                        if (this.eftId != 0)
                        {
                            DbWithBasicFunction dbc = new DbWithBasicFunction();
                            var eftItem = dbc.db.tbl_bankEft.Include("tbl_bank").Where(a => a.bankEftId == this.eftId).FirstOrDefault();
                            if (eftItem != null)
                            {
                                val += " " + eftItem.tbl_bank.name;
                            }
                        }

                        break;
                }


                return val;

            } 
        }

    }

    public class tbl_orderMeta
    {

        public int orderId { get; set; }
        public string orderNo { get; set; }
        public int cargoId { get; set; }
        public int userId { get; set; }
        public int deliveryAddressId { get; set; }
        public int billingAddressId { get; set; }
        public int paymentTypeId { get; set; }
        public decimal cargoPrice { get; set; }

        [Display(Name = "Sipariş Durumu")]
        public int orderStatu { get; set; }

        [Display(Name = "Son Gönderilen Mail")]
        public int orderMailStatu { get; set; }
        public decimal totalProductPrice { get; set; }
        public int trackInfoId { get; set; }
        public string discountCode { get; set; }
        public decimal discountAmount { get; set; }
        public decimal transferDiscount { get; set; }
        public decimal additionalPrice { get; set; }

        [Display(Name = "Ödeme Tutarı")]
        public decimal totalCheckoutPrice { get; set; }

        public int creditPaymentCount { get; set; }
        public bool isRegisteredOrder { get; set; }

        [Display(Name = "Tarih")]
        public System.DateTime createDate { get; set; }

        public string orderGuid { get; set; }

        [AllowHtml]
        public string salesAgreement { get; set; }

        [AllowHtml]
        public string preSalesAgreement { get; set; }

        [AllowHtml]
        public string deliveryAddressObj { get; set; }

        [AllowHtml]
        public string billingAddressObj { get; set; }

    }
}