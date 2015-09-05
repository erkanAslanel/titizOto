using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;
using titizOto.Models;
using ViewModel.Basket;
using ViewModel.Shared;

namespace HelperSite.Shared
{
    public class orderShared : baseShared
    {
        public orderShared(titizOtoEntities db)
        {
            this.db = db;
        }

        public tbl_order getOrderByGuid(string guid)
        {

            return db.tbl_order.Where(a => a.orderGuid == guid).FirstOrDefault();
        }

        internal List<tbl_order> getOrderList(int userId, bool isRegisteredUser)
        {
            return db.tbl_order.Where(a => a.userId == userId && a.isRegisteredOrder == isRegisteredUser).ToList();
        }

        public string getOrderStatuString(orderStatu statu)
        {
            switch (statu)
            {
                case orderStatu.waitPayment:
                    return lang.orderStatuWaitPayment;

                case orderStatu.approved:
                    return lang.orderStatuWaitPayment;

                case orderStatu.preparing:
                    return lang.orderStatuPreparing;

                case orderStatu.leadTime:

                    return lang.orderStatuLeadTime;

                case orderStatu.onCargo:

                    return lang.orderStatuOnCargo;

                case orderStatu.delivered:

                    return lang.orderStatuDelivered;

                case orderStatu.cancel:

                    return lang.cancel;
                default:
                    return "";
            }


        }

        public string getOrderStatuString(int statuId)
        {
            if (statuId > 0 && statuId < 10)
            {
                return getOrderStatuString((orderStatu)statuId);
            }
            else
            {
                return "";
            }

        }

        public tbl_order getUnregisteredOrder(string email, string orderNo)
        {
            var tranckItemList = db.tbl_trackInfo.Where(a => a.email == email).Select(a => a.trackInfoId).ToList();

            if (tranckItemList != null && tranckItemList.Count > 0)
            {
                return db.tbl_order.Where(a => a.orderNo == orderNo && a.isRegisteredOrder == false).ToList().Where(a => tranckItemList.Contains(a.trackInfoId)).FirstOrDefault();
            }
            else
            {
                return null;
            }

        }

        internal orderSummary getOrderSummary(tbl_order orderItem)
        {
            checkoutShared cs = new checkoutShared(db);
            if (orderItem == null)
            {
                return null;
            }

            var culturePrice = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            orderSummary helperItem = new orderSummary();

            helperItem.additionalPrice = orderItem.additionalPrice;
            helperItem.additionalPriceStr = orderItem.additionalPrice.ToString("F2", culturePrice);

            helperItem.allTotalPriceStr = orderItem.totalCheckoutPrice.ToString("F2", culturePrice);
            helperItem.productPriceStr = orderItem.totalProductPrice.ToString("F2", culturePrice);


            helperItem.basketItem.basketList = getBasketContentFromOrder(orderItem);
            helperItem.cargoPrice = orderItem.cargoPrice;


            if (orderItem.isCargoOnCustomer)
            {
                helperItem.cargoPriceStr = lang.checkoutCargoOnCustomer;
            }
            else
            {
                helperItem.cargoPriceStr = orderItem.cargoPrice.ToString("F2", culturePrice) + " TL";
            }


            if (!string.IsNullOrWhiteSpace(orderItem.discountCode))
            {
                helperItem.isDiscountExist = true;
                helperItem.discountCodeString = orderItem.discountCode;
                helperItem.productDiscountPrice = orderItem.discountAmount;
                helperItem.productDiscountPriceStr = orderItem.discountAmount.ToString("F2", culturePrice);
            }


            helperItem.paymentOptionChoose = (ViewModel.Checkout.Payment.paymentOption)orderItem.paymentTypeId;



            switch (helperItem.paymentOptionChoose)
            {

                case ViewModel.Checkout.Payment.paymentOption.transfer:

                    helperItem.paymentOptionChooseStr = lang.checkoutTransfer;

                    if (orderItem.transferDiscount != 0)
                    {
                        helperItem.isTransferDiscountExist = true;
                        helperItem.transferDiscount = orderItem.transferDiscount;
                        helperItem.transferDiscountStr = orderItem.transferDiscount.ToString("F2", culturePrice) + " TL";
                    }

                    break;

                case ViewModel.Checkout.Payment.paymentOption.creditCard:

                    // Additional Price
                    if (orderItem.creditPaymentCount > 1)
                    {
                        helperItem.paymentOptionChooseStr = helperItem.paymentOptionChooseStr + "(" + orderItem.creditPaymentCount + lang.checkoutInstallment + ")";
                    }
                    else
                    {
                        helperItem.paymentOptionChooseStr = lang.checkoutCrediCard + (lang.checkoutCash);
                    }

                    break;
            }




            return helperItem;
        }

        private List<basketItem> getBasketContentFromOrder(tbl_order orderItem)
        {

            var orderDetailList = db.tbl_orderDetail.Where(a => a.orderId == orderItem.orderId).ToList();

            List<basketItem> helper = new List<basketItem>();

            foreach (var item in orderDetailList)
            {
                basketItem helperItem = new basketItem();

                helperItem.description = item.nameWithOption;
                helperItem.productPriceDec = item.productPrice;
                helperItem.quantity = item.quantity;
                helperItem.productTotalPriceDec = item.productTotalPrice;
                helperItem.photo = item.photo;

                helper.Add(helperItem);
            }


            return helper;


        }

        public List<int> getWaitingOrderStatuList()
        {
            List<int> helper = new List<int>();

            helper.Add((int)orderStatu.waitPayment);
            helper.Add((int)orderStatu.approved);
            helper.Add((int)orderStatu.preparing);
            helper.Add((int)orderStatu.leadTime);
            helper.Add((int)orderStatu.onCargo);


            return helper;
        }

        public string getOrderDetailLink(string orderGuid, int langId, string langCode)
        {
            pageShared ps = new pageShared(db);
            var orderDetailPage = ps.getPageByType(pageType.accountOrderDetail, langId);


            if (orderDetailPage != null)
            {
                return langCode + "/" + orderDetailPage.url + ".html?orderGuid=" + orderGuid;
            }
            else
            {
                return "";
            }
        }

        public string getCargoTrackHtml(tbl_order orderItem)
        {
            var cargoItem = db.tbl_cargo.Where(a => a.cargoId == orderItem.cargoId).FirstOrDefault();

            if (cargoItem != null)
            {
                string cargoHtml = "<span class=\"cargoTrackLabel\">" + lang.cargoTrack + ": </span>" + "<a class=\"cargoTrackLink\" rel=\"nofollow\" target=\"_blank\" href=\"[trackLink]\">" + cargoItem.name + "</a>" + "<span class=\"cargoTrackLabel\">" + "Takip No" + ": </span>" + "<span>" + orderItem.shipmentNo + "</span>";

                cargoHtml = cargoHtml.Replace("[trackLink]", cargoItem.trackUrl);

                return cargoHtml;
            }
            else
            {
                return "";
            }

        }
    }

    public enum orderStatu
    {
        waitPayment = 1,
        approved = 2,
        preparing = 3,
        leadTime = 4,
        onCargo = 5,
        delivered = 6,
        cancel=7

    }


}