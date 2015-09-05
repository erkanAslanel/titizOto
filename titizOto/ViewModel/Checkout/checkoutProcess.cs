using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;
using titizOto.App_GlobalResources;
using ViewModel.Checkout.Delivery;
using ViewModel.Checkout.Payment;
using HelperSite.Shared;
using System.Xml.Serialization;

namespace ViewModel.Checkout
{
    public class checkoutProcess
    {
        // Constrain
        public checkoutProcess(List<checkoutPageItem> stepLinkList, topCart cartItem)
        {
            this.stepLinkList = stepLinkList;
            this.cartItem = cartItem;
            this.transferInfo = new transferInfo();
            this.cardInfo = new cardInfo();
        }


        // Option
        public registerOption registerOption { get; set; }
        public checkoutStep currentStep { get; set; }
        public checkoutStep lastSuccessStep { get; set; }

        public topCart cartItem { get; set; }
        public bool isCartItemChange { get; set; }
        public List<checkoutPageItem> stepLinkList { get; set; }

        // Guest User Track  
        public deliveryTrackInfo trackInfo { get; set; }

        // Delivery
        public int deliveryAddressId { get; set; }
        public titizOto.Models.tbl_address deliveryAddress { get; set; }
        public bool isBillingSameAddress { get; set; }

        // Billing
        public int billingAddressId { get; set; }
        public titizOto.Models.tbl_address billingAddress { get; set; }

        // Cargo
        public int cargoId { get; set; }

        // PaymentOption
        public paymentOption paymentOptionChoose { get; set; }

        // Transfer 
        public transferInfo transferInfo { get; set; }

        // CreditCard
        public cardInfo cardInfo { get; set; }

        // Summary
        public string orderNote { get; set; }

        // Validation
        public bool isRegisterOptionsValid()
        {
            //  Kayıtlı üyelik varsa üyelik seçimi OK
            if (cartItem.userId != 0)
            {
                return true;
            }
            else
            {
                if (this.registerOption == Checkout.registerOption.guestUser)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        public Tuple<bool, string> isDeliveryValid(titizOto.Models.titizOtoEntities db)
        {
            HelperSite.Shared.addressShared ads = new HelperSite.Shared.addressShared(db);

            // Unregistered User
            if (!cartItem.isRegisteredUser)
            {
                // Validate Track Info, name,surname,email
                var validateTrackInfo = ads.isValidTrackData(this.trackInfo);

                if (!validateTrackInfo.Item1)
                {
                    return validateTrackInfo;
                }

                // Validate Address 
                var addressValidation = ads.isValidAddress(this.deliveryAddress);

                if (!addressValidation.Item1)
                {
                    return addressValidation;
                }

            }
            else  // Registered User
            {
                var addressItem = ads.getAddressById(this.deliveryAddressId);

                if (addressItem == null || addressItem.statu == false)
                {
                    return new Tuple<bool, string>(false, lang.checkoutDeliveryRequired);
                }

            }


            return new Tuple<bool, string>(true, null);

        }
        public Tuple<bool, string> isBillingValid(titizOto.Models.titizOtoEntities db)
        {
            HelperSite.Shared.addressShared ads = new HelperSite.Shared.addressShared(db);

            // Unregistered User
            if (!cartItem.isRegisteredUser)
            {
                // Validate Address 
                var addressValidation = ads.isValidAddress(this.billingAddress);

                if (!addressValidation.Item1)
                {
                    return addressValidation;
                }
            }
            else
            {
                var addressItem = ads.getAddressById(this.billingAddressId);

                if (addressItem == null || addressItem.statu == false)
                {
                    return new Tuple<bool, string>(false, lang.checkoutDeliveryRequired);
                }

            }

            return new Tuple<bool, string>(true, null);
        }
        public Tuple<bool, string> isCargoValid(titizOto.Models.titizOtoEntities db)
        {
            if (db.tbl_cargo.Where(a => a.statu).Any(a => a.cargoId == this.cargoId))
            {
                return new Tuple<bool, string>(true, null);
            }
            else
            {
                return new Tuple<bool, string>(false, null);
            }
        }
        private Tuple<bool, string> isPaymentValid(titizOto.Models.titizOtoEntities db)
        {
            switch (this.paymentOptionChoose)
            {
                case paymentOption.noAnswer:
                    return new Tuple<bool, string>(false, null);


                case paymentOption.transfer:

                    if (this.transferInfo != null)
                    {
                        var eftItem = db.tbl_bankEft.Where(a => a.bankEftId == this.transferInfo.selectedTransferId).FirstOrDefault();

                        if (eftItem != null)
                        {
                            return new Tuple<bool, string>(true, null);
                        }
                    }

                    return new Tuple<bool, string>(false, null);

                case paymentOption.creditCard:

                    if (this.cardInfo != null)
                    {
                        checkoutShared cs = new checkoutShared(db);
                        var cardInfoValidation = cs.isCardInfoValid(this.cardInfo);
                        return cardInfoValidation;
                    }

                    break;

            }

            return new Tuple<bool, string>(false, null);

        }


        /// <summary>
        /// bool - isValid , string - Message , string - redirectStep
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, string, checkoutStep> validationOnCurrentStep(titizOto.Models.titizOtoEntities db)
        {
            return validationOnCurrentStep(db, this.currentStep);
        }
        public Tuple<bool, string, checkoutStep> validationOnCurrentStep(titizOto.Models.titizOtoEntities db, checkoutStep step)
        {
            if (this.isCartItemChange)
            {
                return new Tuple<bool, string, checkoutStep>(false, null, checkoutStep.none);
            }

            var formerValidation = returnValidResult();
            var currentValidation = returnValidResult();

            int lastSuccessPage = (int)lastSuccessStep;
            int currentPage = (int)step;

            // Access Validation
            if (currentPage > lastSuccessPage + 1)
            {
                checkoutStep lastAccessStep = (checkoutStep)(lastSuccessPage + 1);
                return new Tuple<bool, string, checkoutStep>(false, null, lastAccessStep);
            }



            switch (step)
            {
                case checkoutStep.delivery:

                    if (!isRegisterOptionsValid())
                    {
                        return new Tuple<bool, string, checkoutStep>(false, null, checkoutStep.registerOptions);
                    }

                    break;

                case checkoutStep.billing:

                    formerValidation = validationOnCurrentStep(db, checkoutStep.delivery);

                    if (!formerValidation.Item1)
                    {
                        return formerValidation;
                    }

                    var deliveryValidation = isDeliveryValid(db);

                    if (!deliveryValidation.Item1)
                    {
                        return new Tuple<bool, string, checkoutStep>(false, deliveryValidation.Item2, checkoutStep.delivery);
                    }

                    break;

                case checkoutStep.cargo:

                    formerValidation = validationOnCurrentStep(db, checkoutStep.billing);

                    if (!formerValidation.Item1)
                    {
                        return formerValidation;
                    }
                    var billingValidation = isBillingValid(db);

                    if (!billingValidation.Item1)
                    {
                        return new Tuple<bool, string, checkoutStep>(false, billingValidation.Item2, checkoutStep.billing);
                    }


                    break;

                case checkoutStep.payment:

                    formerValidation = validationOnCurrentStep(db, checkoutStep.cargo);

                    if (!formerValidation.Item1)
                    {
                        return formerValidation;
                    }
                    var cargoValidation = isCargoValid(db);

                    if (!cargoValidation.Item1)
                    {
                        return new Tuple<bool, string, checkoutStep>(false, cargoValidation.Item2, checkoutStep.cargo);
                    }

                    break;
                case checkoutStep.summary:

                    formerValidation = validationOnCurrentStep(db, checkoutStep.payment);

                    if (!formerValidation.Item1)
                    {
                        return formerValidation;
                    }

                    var paymentValidation = isPaymentValid(db);

                    if (!paymentValidation.Item1)
                    {
                        return new Tuple<bool, string, checkoutStep>(false, paymentValidation.Item2, checkoutStep.payment);
                    }

                    break;


            }



            return returnValidResult();


        }
        private Tuple<bool, string, checkoutStep> returnValidResult()
        {
            return new Tuple<bool, string, checkoutStep>(true, null, checkoutStep.none);
        }

        // Clear Data
        public void clearDeliveryData()
        {
            this.deliveryAddressId = 0;
            this.deliveryAddress = null;
        }
        public void clearBillingData()
        {
            this.billingAddressId = 0;
            this.billingAddress = null;
            this.isBillingSameAddress = false;
        }
        public void clearCargoData()
        {
            this.cargoId = 0;
        }
        public void clearPayment()
        {
            this.cardInfo = new cardInfo();
            this.transferInfo = new transferInfo();
            this.paymentOptionChoose = paymentOption.noAnswer;
        }


        public void clearDataOnStepAndBindCurrentStep(checkoutStep step)
        {
            this.currentStep = step;

            switch (step)
            {
                case checkoutStep.registerOptions:

                    clearDeliveryData();
                    clearBillingData();
                    clearCargoData();
                    clearPayment();


                    break;

                case checkoutStep.delivery:

                    clearBillingData();
                    clearCargoData();
                    clearPayment();

                    break;

                case checkoutStep.billing:
                    clearCargoData();
                    clearPayment();


                    break;

                case checkoutStep.cargo:
                    clearPayment();

                    break;





            }

        }

        
    }

    public enum registerOption
    {
        noAnswer = 0,
        guestUser = 1,
        registerOrLogin = 2
    }

    public enum checkoutStep
    {
        none = -1,
        registerOptions = 0,
        delivery = 1,
        billing = 2,
        cargo = 3,
        payment = 4,
        summary = 5,
        complete = 6,
        error = 7

    }

}