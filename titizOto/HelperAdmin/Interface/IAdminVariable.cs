using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelperAdmin
{
    interface IAdminVariable
    {
         AdminVariable getAdminVariable(int userId);
    }
}
