using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titizOto.Models;
using ViewModel.Account.Address;
using titizOto.App_GlobalResources;
using System.ComponentModel.DataAnnotations;
using ViewModel.Checkout.Delivery;
using HelperSite.DbController;
using Newtonsoft.Json;

namespace HelperSite.Shared
{
    public class addressShared : baseShared
    {
        public addressShared(titizOtoEntities db)
        {
            this.db = db;
        }

        public List<tbl_address> getAddressListByUserId(int userId)
        {
            return db.tbl_address.Where(a => a.userId == userId && a.statu == true && a.isGuestUser == false).ToList();
        }

        public tbl_address getAddressById(int addressId)
        {
            return db.tbl_address.Where(a => a.addressId == addressId).FirstOrDefault();

        }

        public tbl_address getAddresByUserId(int userId)
        {
            return db.tbl_address.Where(a => a.userId == userId && a.statu == true && a.isGuestUser == false).FirstOrDefault();

        }

        public tbl_address getAddresByUserIdWithDeleted(int userId)
        {
            return db.tbl_address.Where(a => a.userId == userId && a.isGuestUser == false).FirstOrDefault();
        }

        // Main
        private addressItem getAddressAccountTemplate(tbl_address item)
        {
            addressItem tempItem = new addressItem();

            if (item != null)
            {
                tempItem.addressId = item.addressId;
                tempItem.name = item.name;

                if (item.isPersonal)
                {
                    tempItem.type = lang.personal;
                }
                else
                {
                    tempItem.type = lang.corporate;
                }

                tempItem.content = item.address + "<br />" + item.district + "-" + item.city;
            }

            return tempItem;
        }

        // Helper  getAddressAccountTemplate
        public List<addressItem> getAddressListTemplate(List<tbl_address> itemList)
        {

            List<addressItem> tempList = new List<addressItem>();

            foreach (var item in itemList)
            {
                tempList.Add(getAddressAccountTemplate(item));
            }

            return tempList;
        }

        // Helper  getAddressAccountTemplate
        public List<addressItem> getAddressListTemplate(int userId)
        {
            return getAddressListTemplate(getAddressListByUserId(userId));

        }

        public void deleteAddressById(int addressId)
        {

            var item = db.tbl_address.Where(a => a.addressId == addressId).FirstOrDefault();

            if (item != null)
            {
                db.tbl_address.Remove(item);
                db.SaveChanges();
            }
        }

        public void updateAddressIsDeleted(int addressId)
        {
            var item = db.tbl_address.Where(a => a.addressId == addressId).FirstOrDefault();

            if (item != null)
            {
                item.statu = false;
                db.SaveChanges();
            }


        }

        public Tuple<bool, string> isValidAddress(tbl_address item)
        {
            var validationContext = new ValidationContext(item, null, null);
            var validationResult = new List<ValidationResult>();
            Validator.TryValidateObject(item, validationContext, validationResult, true);

            return getValidationResult(validationResult);
        }

        public Tuple<bool, string> isValidTrackData(deliveryTrackInfo item)
        {
            var validationContext = new ValidationContext(item, null, null);
            var validationResult = new List<ValidationResult>();
            Validator.TryValidateObject(item, validationContext, validationResult, true);

            return getValidationResult(validationResult);

        }

        public string getAddressHtml(int addressId, AddressHtmlType addressType, DbWithControllerWithMaster helperController)
        {
            string addressHtml = "";
            var addressItem = db.tbl_address.Where(a => a.addressId == addressId).FirstOrDefault();
            addressHtml = getAddressHtml(addressItem, addressType, helperController);

            return addressHtml;
        }

        public string getAddressHtml(tbl_address addressItem, AddressHtmlType addressType, DbWithControllerWithMaster helperController)
        {
            if (addressItem != null)
            {

                switch (addressType)
                {
                    case AddressHtmlType.agreement:
                        var item = getAddressAccountTemplate(addressItem);
                        return item.content;

                    case AddressHtmlType.mail:
                    case AddressHtmlType.orderDetail:

                        return helperController.RenderRazorViewToString("AddressMailHtml", addressItem);

                    case AddressHtmlType.adminOrderDetail:
                        return "";
                }


            }

            return "";

        }

        public string getAddressHtml(tbl_address addressItem, AddressHtmlType addressType, titizOto.Areas.radmin.Controllers.OrderController helperController)
        {
            if (addressItem != null)
            {

                switch (addressType)
                {
                    case AddressHtmlType.agreement:
                        var item = getAddressAccountTemplate(addressItem);
                        return item.content;

                    case AddressHtmlType.mail:
                    case AddressHtmlType.orderDetail:

                        return helperController.RenderRazorViewToString("AddressMailHtml", addressItem);

                    case AddressHtmlType.adminOrderDetail:
                        return helperController.RenderRazorViewToString("AddressAdminHtml", addressItem);
                       
                }


            }

            return "";

        }

        internal tbl_address getAddressHtmlFromObj(int addressId, string addressObj)
        {

            tbl_address addressItem = null;

            if (!string.IsNullOrWhiteSpace(addressObj))
            {
                addressItem = JsonConvert.DeserializeObject<tbl_address>(addressObj, new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               });
            }

            if (addressItem == null)
            {
                addressItem = db.tbl_address.Where(a => a.addressId == addressId).FirstOrDefault();
            }

            return addressItem;

        }

        public tbl_address addAddress(tbl_address item)
        {
            db.tbl_address.Add(item);
            db.SaveChanges();
            return item;
        }

        internal string getAddresPhoneByAddressId(int addressId)
        {
            var item = getAddressById(addressId);

            if (item != null)
            {
                return item.phone;
            }
            else
            {
                return "";
            }
        }
    }

    public enum AddressHtmlType
    {

        mail, orderDetail, agreement, adminOrderDetail


    }


}