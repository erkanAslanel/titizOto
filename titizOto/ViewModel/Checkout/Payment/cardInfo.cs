using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;

namespace ViewModel.Checkout.Payment
{
    public class cardInfo
    {
        public cardInfo()
        {
            this.cardOption = new cardOption();
            this.cardOption.creditOptionId = -1;
        }

        //--creditCard -->
        [RegularExpression(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[47][0-9]{13})$", ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutCardInfoCreditCardRequired")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutCardInfoCreditCardRequired")]
        [Display(ResourceType = typeof(lang), Name = "checkoutCardInfoCreditCard")]
        [DataType("normalText")]
        public string creditCard { get; set; }

        //--month -->
        [Display(ResourceType = typeof(lang), Name = "formMonth")]
        [Range(1, 12, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formMonthRequired")]
        [DataType("dropDown")]
        public int month { get; set; }

        //--year -->
        [Display(ResourceType = typeof(lang), Name = "formYear")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formYearRequired")]
        [DataType("dropDown")]
        public int year { get; set; }

        //--cvv -->
        [Display(ResourceType = typeof(lang), Name = "checkoutCardInfoCvv")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutCardInfoCvvRequired")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutCardInfoCvvRequired")]
        [DataType("normalText")]
        public int cvv { get; set; }


        [Display(ResourceType = typeof(lang), Name = "checkoutCardName")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "checkoutCardNameRequired")]
        public string nameSurname { get; set; }

        public Dictionary<int, string> monthList
        {
            get
            {

                var helper = new Dictionary<int, string>();

                helper.Add(0, "Ay");

                for (int i = 1; i < 13; i++)
                {
                    if (i.ToString().Length == 1)
                    {
                        helper.Add(i, "0" + i.ToString());
                    }
                    else
                    {

                        helper.Add(i, i.ToString());
                    }
                }


                return helper;
            }
        }
        public Dictionary<int, string> yearList
        {
            get
            {
                var helper = new Dictionary<int, string>();
                helper.Add(0, "Yıl");
                for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 10; i++)
                {
                    helper.Add(i, i.ToString());
                }

                return helper;
            }
        }

        public cardOption cardOption { get; set; }

        public string totalPriceStr { get; set; }

        public bool isErrorExist { get; set; }
        public string message { get; set; }

    }
}