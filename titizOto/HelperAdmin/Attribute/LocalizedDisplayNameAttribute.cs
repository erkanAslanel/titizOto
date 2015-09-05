using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;

namespace HelperAdmin
{
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string resourceId)
            : base(GetMessageFromResource(resourceId))
        { }

        private static string GetMessageFromResource(string resourceId)
        {
            ResourceManager ResManager = new ResourceManager("Resources.lang",
                System.Reflection.Assembly.Load("App_GlobalResources"));
            return ResManager.GetString(resourceId);

          
        }
    }
}