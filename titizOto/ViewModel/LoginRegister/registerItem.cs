using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using titizOto.App_GlobalResources;



namespace ViewModel.LoginRegister
{

    public class registerItem
    {
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formNameRequired")]
        [Display(ResourceType = typeof(lang), Name = "formName")]
        public string name { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formSurnameRequired")]
        [Display(ResourceType = typeof(lang), Name = "formSurname")]
        public string surname { get; set; }


        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        [Display(ResourceType = typeof(lang), Name = "formEmail")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessageResourceType =  typeof(lang), ErrorMessageResourceName = "formEmailRequired")]
        public string email { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formPasswordRequired")]
        [Display(ResourceType = typeof(lang), Name = "formPassword")]
        [StringLength(16, MinimumLength = 6, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "passwordLength")]
        public string password { get; set; }

        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formPassworRepRequired")]
        [Display(ResourceType = typeof(lang), Name = "formPassworRep")]
        [StringLength(16, MinimumLength = 6, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "passwordLength")]
        public string passwordRep { get; set; }

        [Display(ResourceType = typeof(lang), Name = "formDay")]
        [Range(1, 31, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formDayRequired" )]
        public int day { get; set; }

        [Display(ResourceType = typeof(lang), Name = "formMonth")]
        [Range(1, 12, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formMonthRequired")]
        public int month { get; set; }

        [Display(ResourceType = typeof(lang), Name = "formYear")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formYearRequired")]
        public int year { get; set; }

        [Display(ResourceType = typeof(lang), Name = "formGender")]
        [Required(ErrorMessageResourceType = typeof(lang), ErrorMessageResourceName = "formGenderRequired")]
        public int gender { get; set; }

        [Display(ResourceType = typeof(lang), Name = "formUserDocument")]
        public string aggrementUrl { get; set; }
        public bool isAggrementCheck { get; set; }

        public List<Tuple<int, string>> dayList
        {
            get
            {

                var helper = new List<Tuple<int, string>>();
                helper.Add(new Tuple<int, string>(0, "Gün"));

                for (int i = 1; i < 32; i++)
                {
                    if (i.ToString().Length == 1)
                    {
                        helper.Add(new Tuple<int, string>(i, "0" + i.ToString()));

                    }
                    else
                    {



                        helper.Add(new Tuple<int, string>(i, i.ToString()));

                    }
                }

                return helper;
            }
        }

        public List<Tuple<int, string>> monthList
        {
            get
            {

                var helper = new List<Tuple<int, string>>();

                helper.Add(new Tuple<int, string>(0, "Ay"));

                for (int i = 1; i < 13; i++)
                {
                    if (i.ToString().Length == 1)
                    {
                        helper.Add(new Tuple<int, string>(i, "0" + i.ToString()));
                    }
                    else
                    {

                        helper.Add(new Tuple<int, string>(i, i.ToString()));
                    }
                }


                return helper;
            }
        }

        public List<Tuple<int, string>> yearList
        {
            get
            {
                var helper = new List<Tuple<int, string>>();
                helper.Add(new Tuple<int, string>(0, "Yıl"));
                for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 100; i--)
                {
                    helper.Add(new Tuple<int, string>(i, i.ToString()));
                }

                return helper;
            }
        }

        public bool isMessageExist { get; set; }

        public string message { get; set; }

        public string agreementContent { get; set; }
    }


}