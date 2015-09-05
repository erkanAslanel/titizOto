using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace HelperAdmin
{
    public class DashBoardStatistic
    {

        public DashBoardStatistic()
        {
            this.orderList = new List<tbl_order>();
        }

        public string categoryCount { get; set; } 
        public string sliderCount { get; set; } 
        public List<tbl_order> orderList { get; set; }


        public int dayOrder { get; set; }
        public int dayUser { get; set; }
        public int dayNewsletter { get; set; }

        public int allOrder { get; set; }
        public int allCategory { get; set; }
        public int allProduct { get; set; }
        public int allPage { get; set; }
        public int allUser{ get; set; }
        public int allNewsletter { get; set; }

    }
}