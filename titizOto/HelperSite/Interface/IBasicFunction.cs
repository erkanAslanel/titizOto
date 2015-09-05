using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelperSite.Interface
{
    interface IBasicFunction
    {  

         void errorSend(Exception ex, string msg);
         string getSuccesMessage(string text,string className);
         string getErrorMessage(string text, string className);
    }
}
