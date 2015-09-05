using HelperSite.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelperSite.Pos
{
    public class garantiPos : posShared , IPos
    { 
        public bool isBinExist(string cardNo)
        {
            return true;
        }
    }
}