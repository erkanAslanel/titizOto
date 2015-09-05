using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace titizOto.Models
{
    [MetadataType(typeof(tbl_orderDetailMeta))]
    public partial class tbl_orderDetail
    {


    }
    public class tbl_orderDetailMeta
    {
        [AllowHtml]
        public string nameWithOption { get; set; }

        [AllowHtml]
        public string photo { get; set; }
    }
}