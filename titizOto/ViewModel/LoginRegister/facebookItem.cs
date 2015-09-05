using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.LoginRegister
{
    public class facebookItem
    {
        public facebookItem(dynamic result)
        {

            if (result != null)
            {
                if (result.email != null)
                {
                    this.email = result.email;
                }

                if (result.first_name != null)
                {
                    this.firstName = result.first_name;
                }

                if (result.gender != null)
                {
                    this.strGender = result.gender;
                }

                if (result.last_name != null)
                {
                    this.last_name = result.last_name;
                }

                if (result.birthday != null)
                {
                    this.strBirthday = result.birthday;
                }
            }
        }

        public string email { get; set; }
        public string firstName { get; set; }
        public string last_name { get; set; }
        public string strGender { get; set; }
        public string strBirthday { get; set; }

        public int gender
        {
            get
            {

                if (string.IsNullOrWhiteSpace(this.strGender))
                {
                    return 0;
                }
                else
                {
                    if (this.strGender=="male")
                    {
                        return 1;
                    }

                    if (this.strGender=="female")
                    {
                         return 2;
                    }

                    return 0;
                }


            }
        }
        public DateTime birthday
        {
            get
            {
                DateTime parsedTime = DateTime.Now;
                string pattern = "MM/DD/YYYY";

                if (string.IsNullOrWhiteSpace(this.strBirthday))
                {
                    return parsedTime;
                }


                DateTime.TryParseExact(this.strBirthday, pattern, null, System.Globalization.DateTimeStyles.None, out parsedTime);

                return parsedTime;


            }
        }
    }
}