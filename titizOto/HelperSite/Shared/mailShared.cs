using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace HelperSite.Shared
{
    public class mailShared : baseShared
    {

        private int langId { get; set; }

        public mailShared(titizOtoEntities db, int langId)
        {
            this.db = db;
            this.langId = langId;
        }

        public Tuple<string, string> getActivationMailContent(string name, string surname, string activationCode, string basePath, string langCode)
        {

            var item = getEmailByTypeIdAndLang((int)mailType.activation, this.langId);

            if (item != null)
            {
                string mailContent = item.detail;

                var pageItem = db.tbl_page.Where(a => a.langId == langId && a.pageTypeId == (int)pageType.activation).FirstOrDefault();

                if (pageItem != null)
                {
                    string pageUrl = basePath + langCode + "/" + pageItem.url + "/" + activationCode;

                    mailContent = mailContent.Replace("[registerName]", name);
                    mailContent = mailContent.Replace("[registerSurname]", surname);
                    mailContent = mailContent.Replace("[activationLink]", pageUrl);


                    return new Tuple<string, string>(item.title, mailContent);
                }
                else
                {
                    return returnEmpty();
                }
            }
            else
            {
                return returnEmpty();
            }


        }

        public Tuple<string, string> getResetPasswordMailContent(string name, string surname, string resetCode, string basePath, string langCode)
        {

            var item = getEmailByTypeIdAndLang((int)mailType.resetPassword, this.langId);

            if (item != null)
            {
                string mailContent = item.detail;

                var pageItem = db.tbl_page.Where(a => a.langId == langId && a.pageTypeId == (int)pageType.resetPassword).FirstOrDefault();

                if (pageItem != null)
                {
                    string pageUrl = basePath + langCode + "/" + pageItem.url + "/" + resetCode;

                    mailContent = mailContent.Replace("[registerName]", name);
                    mailContent = mailContent.Replace("[registerSurname]", surname);
                    mailContent = mailContent.Replace("[resetLink]", pageUrl);


                    return new Tuple<string, string>(item.title, mailContent);
                }
                else
                {
                    return returnEmpty();
                }
            }
            else
            {
                return returnEmpty();
            }


        }

        public Tuple<string, string> getRegisterThankMailContent(string name, string surname)
        {
            var item = getEmailByTypeIdAndLang((int)mailType.thanksRegister, this.langId);

            if (item != null)
            {
                string mailContent = item.detail;
                mailContent = mailContent.Replace("[registerName]", name);
                mailContent = mailContent.Replace("[registerSurname]", surname);

                return new Tuple<string, string>(item.title, mailContent);
            }
            else
            {
                return returnEmpty();
            }
        }

        public Tuple<string, string> getTransferMailContent(string nameSurname, string orderNo, string transferInfo, string deliveryAddress, string billingAddress, string orderInfo)
        {
            var item = getEmailByTypeIdAndLang((int)mailType.transferOrder, this.langId);

            if (item != null)
            {
                string mailContent = item.detail;
                mailContent = mailContent.Replace("[orderNo]", orderNo);
                mailContent = mailContent.Replace("[registerNameSurname]", nameSurname);
                mailContent = mailContent.Replace("[transferInfo]", transferInfo);
                mailContent = mailContent.Replace("[deliveryAddress]", deliveryAddress);
                mailContent = mailContent.Replace("[billingAddress]", billingAddress);
                mailContent = mailContent.Replace("[orderInfo]", orderInfo);

                return new Tuple<string, string>(item.title + " #" + orderNo, mailContent);
            }
            else
            {
                return returnEmpty();
            }
        }

        public Tuple<string, string> getOrderStatu(string nameSurname, string orderNo,string orderStatu,string orderLink)
        {
                var item = getEmailByTypeIdAndLang((int)mailType.statuUpdate, this.langId);


                if (item != null)
                {
                    string mailContent = item.detail;
                    mailContent = mailContent.Replace("[orderNo]", orderNo);
                    mailContent = mailContent.Replace("[registerNameSurname]", nameSurname);
                    mailContent = mailContent.Replace("[orderStatu]", orderStatu);
                    mailContent = mailContent.Replace("[orderLink]", orderLink);
                  
                    return new Tuple<string, string>(item.title + " #" + orderNo, mailContent);
                }
                else
                {
                    return returnEmpty();
                }

        }

        public Tuple<string, string> getMinStockMail(string minStockProductListHtml)
        {
            var item = getEmailByTypeIdAndLang((int)mailType.minStock, this.langId);

            if (item != null)
            {
                string mailContent = item.detail;
                mailContent = mailContent.Replace("[minStockProductList]", minStockProductListHtml);
                return new Tuple<string, string>(item.title, mailContent);
            }
            else
            {
                return returnEmpty();
            }
        }

        public Tuple<string, string> getOrderCargo(string nameSurname, string orderNo, string shipmentNo, string shipmentLink, string shipmentName, string orderLink)
        {
            var item = getEmailByTypeIdAndLang((int)mailType.onCargo, this.langId);


            if (item != null)
            {
                string mailContent = item.detail;
                mailContent = mailContent.Replace("[orderNo]", orderNo);
                mailContent = mailContent.Replace("[registerNameSurname]", nameSurname);
                mailContent = mailContent.Replace("[shipmentNo]", shipmentNo);
                mailContent = mailContent.Replace("[shipmentLink]", shipmentLink);
                mailContent = mailContent.Replace("[shipmentName]", shipmentLink); 
                mailContent = mailContent.Replace("[orderLink]", orderLink);

                return new Tuple<string, string>(item.title + " #" + orderNo, mailContent);
            }
            else
            {
                return returnEmpty();
            }
        
        
        }

        private tbl_email getEmailByTypeIdAndLang(int typeId, int langId)
        {
            return db.tbl_email.Where(a => a.langId == langId && a.emailTypeId == typeId).FirstOrDefault();
        }

        private Tuple<string, string> returnEmpty()
        {
            return new Tuple<string, string>("", "");

        }
    }

    public enum mailType
    {
        activation = 1,
        thanksRegister = 2,
        resetPassword = 3,
        transferOrder = 4,
        minStock = 5,
        statuUpdate=6,
        onCargo=7

    }
}