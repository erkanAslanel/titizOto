using HelperSite.DbController;
using HelperSite.Interface;
using HelperSite.Pos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;
using titizOto.Models;
using ViewModel.Basket;
using ViewModel.Checkout;
using ViewModel.Checkout.Cargo;
using ViewModel.Checkout.Payment;
using ViewModel.Checkout.Summary;
using ViewModel.Shared;

namespace HelperSite.Shared
{
    public class checkoutShared : baseShared
    {
        public checkoutShared(titizOtoEntities db)
        {
            this.db = db;
        }

        #region Cargo

        public List<cargoItem> getCargoItemList(decimal basketTotal, int langId, string priceFormat)
        {
            List<cargoItem> helperList = new List<cargoItem>();
            var priceCultureFormet = CultureInfo.CreateSpecificCulture(priceFormat);

            var cargoTable = db.tbl_cargo.Where(a => a.statu).OrderBy(a => a.sequence).ToList();

            foreach (var item in cargoTable)
            {
                cargoItem helper = new cargoItem();

                helper.name = item.name;
                helper.cargoDetail = item.detail;
                helper.cargoId = item.cargoId;
                helper.photo = "ImageShow/cargo/" + item.photo + "/" + item.photoCoordinate + "/150/150/1";

                if (basketTotal <= item.freeCargoPrice)
                {
                    helper.price = 0;
                    helper.priceString = helper.price.ToString("F2", priceCultureFormet);
                }

                if (item.isCargoPriceOnCustomer)
                {
                    helper.isCargoOnCustomer = true;
                }

                helperList.Add(helper);
            }

            return helperList;
        }



        #endregion

        #region Transfer

        internal transferDiscount getTransferInfo(int langId)
        {
            transferDiscount helper = new transferDiscount();
            var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();
            if (settingItem != null)
            {
                if (settingItem.isTransferDiscount.HasValue && settingItem.transferDiscountType.HasValue && settingItem.transferDiscountAmount.HasValue && settingItem.isTransferDiscount.Value)
                {
                    helper.isDiscountExist = true;
                    helper.amount = settingItem.transferDiscountAmount.Value;

                    // Pertange Discount
                    if ((int)transfetDiscountType.percentage == settingItem.transferDiscountType.Value)
                    {
                        helper.discountType = transfetDiscountType.percentage;
                    }

                    // Amount Discount
                    if ((int)transfetDiscountType.amount == settingItem.transferDiscountType.Value)
                    {
                        helper.discountType = transfetDiscountType.amount;
                    }
                }

            }

            return helper;
        }

        internal string getTransferInfoText(transferDiscount item, string currency)
        {
            string text = lang.checkoutTransferDiscount;

            if (item != null && item.isDiscountExist)
            {
                switch (item.discountType)
                {
                    case transfetDiscountType.percentage:
                        text = text.Replace("[amount]", "%" + item.amount.ToString("N0"));
                        break;
                    case transfetDiscountType.amount:
                        text = text.Replace("[amount]", "%" + item.amount.ToString("N0") + " " + currency);
                        break;
                }

                return text;
            }
            else
            {
                return null;
            }


        }

        internal List<tbl_bankEft> getEftList(int langId)
        {
            return db.tbl_bankEft.Include("tbl_bank").Where(a => a.tbl_bank.langId == langId && a.tbl_bank.statu && a.statu).ToList();
        }

        #endregion

        #region Credit

        internal bool isCreditCardValid(string creditCard)
        {
            if (!string.IsNullOrWhiteSpace(creditCard) && System.Text.RegularExpressions.Regex.IsMatch(creditCard, @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[47][0-9]{13})$"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        internal List<cardOptionItem> getCardOptionList(int langId, decimal cargoAndProductPrice, string cardNo, posType postype, CultureInfo priceStrFormat, string currency)
        {
            var posList = getBankPosByType(postype, langId);

            int posId = 0;
            foreach (var item in posList)
            {
                // Get Pos Class
                var posItem = getPosClass(item.posCode);
                if (posItem != null)
                {
                    // Is Card Relative Pos
                    if (posItem.isBinExist(cardNo))
                    {
                        posId = item.bankPosId;
                        break;
                    }
                }
            }

            // Taksitli
            if (posId != 0)
            {
                return getCardOptionListByPosId(posId, postype, langId, cargoAndProductPrice, priceStrFormat, currency);
            }
            else // Tek çekim Main Pos
            {
                return getMainPosOption(postype, langId, cargoAndProductPrice, priceStrFormat, currency);
            }
        }

        private List<cardOptionItem> getCardOptionListByPosId(int posId, posType postype, int langId, decimal cargoAndProductPrice, CultureInfo priceStrFormat, string currency)
        {
            var posItem = getBankPosByType(postype, langId).Where(a => a.bankPosId == posId).FirstOrDefault();

            // posItem is Valid
            if (posItem != null)
            {
                db.Entry(posItem).Collection(p => p.tbl_bankPosOption).Load();
                var posOptionList = posItem.tbl_bankPosOption;

                List<cardOptionItem> helperList = new List<cardOptionItem>();

                // Add Defult Option => Tek Çekim
                helperList.Add(getDefaultOption(cargoAndProductPrice, priceStrFormat, currency));


                foreach (var item in posOptionList)
                {
                    if (item.minBasketAmount <= cargoAndProductPrice)
                    {
                        cardOptionItem helper = new cardOptionItem();
                        helper.monthStr = item.paymentCount.ToString();

                        var calc = getInstallmentAmount(cargoAndProductPrice, item.paymentCount, item.additionalAmount);

                        helper.totalPrice = calc.Item2;
                        helper.monthPrice = calc.Item1;
                        helper.bankPosOptionId = item.bankPosOptionId;
                        helper.totalPriceStr = helper.totalPrice.ToString("F2", priceStrFormat) + " " + currency;
                        helper.monthPriceStr = helper.monthPrice.ToString("F2", priceStrFormat) + " " + currency;

                        helperList.Add(helper);
                    }
                }

                return helperList;
            }
            else
            {
                return null;
            }

        }

        private List<cardOptionItem> getMainPosOption(posType postype, int langId, decimal cargoAndProductPrice, CultureInfo priceStrFormat, string currency)
        {

            var posItem = getBankPosByType(postype, langId).Where(a => a.isMainPos).FirstOrDefault();

            if (posItem != null)
            {
                List<cardOptionItem> helperList = new List<cardOptionItem>();

                // Only Add Defult Option => Tek Çekim
                helperList.Add(getDefaultOption(cargoAndProductPrice, priceStrFormat, currency));
                return helperList;
            }
            else
            {
                return null;
            }
        }

        private cardOptionItem getDefaultOption(decimal cargoAndProductPrice, CultureInfo priceStrFormat, string currency)
        {
            cardOptionItem defaultOption = new cardOptionItem();

            defaultOption.bankPosOptionId = 0;
            defaultOption.monthStr = lang.checkoutCash;
            defaultOption.monthPrice = cargoAndProductPrice;
            defaultOption.totalPrice = cargoAndProductPrice;
            defaultOption.monthPriceStr = "-";
            defaultOption.totalPriceStr = defaultOption.totalPrice.ToString("F2", priceStrFormat) + " " + currency;

            return defaultOption;
        }

        public List<tbl_bankPos> getBankPosByType(posType postype, int iangId)
        {
            return db.tbl_bankPos.Include("tbl_bank").Where(a => a.tbl_bank != null && a.tbl_bank.statu && a.statu && a.posTypeId == (int)postype).ToList();

        }

        public string getSalesAgreement(string customerNameSurname, string customerAdress, string customerPhone, string customerEmail, string customerBasket, string orderDate)
        {
            string content = "";

            var moduleItem = db.tbl_module.Where(a => a.typeId == (int)moduleType.salesAgreement).FirstOrDefault();

            if (moduleItem != null)
            {

                content = moduleItem.htmlContent;

                content = content.Replace("[customerNameSurname]", customerNameSurname);
                content = content.Replace("[customerAdress]", customerAdress);
                content = content.Replace("[customerPhone]", customerPhone);
                content = content.Replace("[customerEmail]", customerEmail);
                content = content.Replace("[customerBasket]", customerBasket);
                content = content.Replace("[orderDate]", orderDate);

            }



            return content;
        }

        public string getPreSalesAgreement(string customerNameSurname, string customerAdress, string customerPhone, string customerEmail, string customerBasket, string orderDate)
        {

            string content = "";

            var moduleItem = db.tbl_module.Where(a => a.typeId == (int)moduleType.preSalesAgreement).FirstOrDefault();

            if (moduleItem != null)
            {

                content = moduleItem.htmlContent;

                content = content.Replace("[customerNameSurname]", customerNameSurname);
                content = content.Replace("[customerAdress]", customerAdress);
                content = content.Replace("[customerPhone]", customerPhone);
                content = content.Replace("[customerEmail]", customerEmail);
                content = content.Replace("[customerBasket]", customerBasket);
                content = content.Replace("[orderDate]", orderDate);

            }



            return content;
        }

        public IPos getPosClass(string code)
        {
            try
            {
                Type type = System.Reflection.TypeDelegator.GetType("HelperSite.Pos." + code.Trim());
                object item = Activator.CreateInstance(type);
                return (IPos)item;


            }
            catch
            {

                return null;
            }

        }

        public Tuple<bool, string> isCardInfoValid(cardInfo item)
        {

            var validationContext = new ValidationContext(item, null, null);
            var validationResult = new List<ValidationResult>();
            Validator.TryValidateObject(item, validationContext, validationResult, true);

            return getValidationResult(validationResult);
        }



        #endregion

        #region CalcPrice

        public Tuple<bool, decimal, string> getProductPrice(topCart cartItem, int langId, string langCode, string mainPath, bool isDeletedInclude)
        {
            // get Basket Content => Price , Discount 
            helperBasket helperBasket = new helperBasket();
            basketShared bs = new basketShared(db);
            var basketContent = bs.getBasketHelperWithProductAndDiscount(cartItem, langId, langCode, mainPath, isDeletedInclude);
            if (basketContent.Item2 == basketActionResult.redirect)
            {
                return new Tuple<bool, decimal, string>(false, 0, "basket");
            }

            helperBasket = basketContent.Item1;
            var basketTotal = helperBasket.totalPriceDec;

            return new Tuple<bool, decimal, string>(true, basketTotal, null);

        }

        internal decimal getCargoPriceByCargoId(int cargoId, decimal basketTotal, int langId)
        {
            var list = getCargoItemList(basketTotal, langId, "en-US");

            if (list.Any(a => a.cargoId == cargoId))
            {
                return list.Where(a => a.cargoId == cargoId).FirstOrDefault().price;
            }
            else
            {
                return -1;
            }
        }



        public Tuple<decimal, decimal> getInstallmentAmount(decimal totalAmount, int payCount, decimal additional)
        {
            totalAmount = Math.Round((totalAmount / 100) * (100 + additional), 2, MidpointRounding.AwayFromZero);

            return new Tuple<decimal, decimal>(Math.Round(totalAmount / payCount, 2, MidpointRounding.AwayFromZero), totalAmount);

        }

        #endregion

        #region Summary

        internal orderInfo getOrderInfoByCheckoutProcess(checkoutProcess checkoutItem, orderSummary helperPage, DbWithControllerWithMaster helperController, BasketHtmlType htmlType, AddressHtmlType addressType, TransferHtmlType transferType, int langId)
        {
            var ads = new addressShared(db);
            var us = new userShared(db);
            var item = new ViewModel.Checkout.Summary.orderInfo();


            // Kayıtlı Üye
            if (checkoutItem.cartItem.isRegisteredUser)
            {

                //customerNameSurname
                item.customerNameSurname = checkoutItem.cartItem.nameSurname;

                //customerEmail
                var userId = checkoutItem.cartItem.userId;
                item.customerEmail = us.getUserById(userId).email;

                //customer Delivery Adress
                item.deliveryHtml = ads.getAddressHtml(checkoutItem.deliveryAddressId, addressType, helperController);

                //customerPhone 
                item.customerPhone = ads.getAddresPhoneByAddressId(checkoutItem.deliveryAddressId);

                // customer Billing Addres
                item.billingHtml = ads.getAddressHtml(checkoutItem.billingAddressId, addressType, helperController);
            }
            else
            {
                //customerNameSurname
                item.customerNameSurname = checkoutItem.trackInfo.name + " " + checkoutItem.trackInfo.surname;

                //customerEmail
                item.customerEmail = checkoutItem.trackInfo.email;

                //customer Delivery Adress
                item.deliveryHtml = ads.getAddressHtml(checkoutItem.deliveryAddress, addressType, helperController);

                //customerPhone 
                item.customerPhone = checkoutItem.deliveryAddress.phone;

                // customer Billing Addres
                item.billingHtml = ads.getAddressHtml(checkoutItem.billingAddress, addressType, helperController);
            }

            item.orderDate = DateTime.Now.ToString("dd.MM.yyyy");
            item.customerBasket = getBasketListWithPlainHtml(helperPage, helperController, htmlType);
            item.transferAccountHtml = getTransferInfoHtml(checkoutItem.transferInfo.selectedTransferId, langId, helperController, transferType);

            return item;
        }

        public string getTransferInfoHtml(int eftId, int langId, DbWithControllerWithMaster helperController, TransferHtmlType transferType)
        {
            var eftItem = getEftList(langId).Where(a => a.bankEftId == eftId).FirstOrDefault();
            if (eftItem != null)
            {
                switch (transferType)
                {
                    case TransferHtmlType.mail:
                    case TransferHtmlType.orderDetail:
                        return helperController.RenderRazorViewToString("TransferMailHtml", eftItem);


                }


            }

            return "";
        }

        private string getBasketListWithPlainHtml(orderSummary helperPage, DbWithControllerWithMaster helperController, BasketHtmlType htmlType)
        {
            switch (htmlType)
            {
                case BasketHtmlType.agreement:
                    return helperController.RenderRazorViewToString("PlainBasket", helperPage);

                case BasketHtmlType.mail:

                    return helperController.RenderRazorViewToString("PlainBasketMail", helperPage);

                default:
                    return "";

            }
        }

        internal string getOrderNo()
        {
            Random rnd = new Random();

            // 7 Digit
            var orderNo = rnd.Next(1000000, 10000000).ToString();

            if (db.tbl_order.Any(a => a.orderNo == orderNo))
            {
                return getOrderNo();
            }
            else
            {
                return orderNo;
            }
        }

        internal tbl_trackInfo addTrackInfo(string name, string surname, string email)
        {
            tbl_trackInfo item = new tbl_trackInfo();

            item.name = name;
            item.surname = surname;
            item.email = email;

            db.tbl_trackInfo.Add(item);
            db.SaveChanges();

            return item;
        }

        #endregion

    }

    public enum BasketHtmlType
    {
        agreement,
        mail
    }

    public enum TransferHtmlType
    {

        mail,
        orderDetail

    }

}