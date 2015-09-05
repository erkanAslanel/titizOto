using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel.Shared;

namespace ViewModel.Search
{
    public class searchResultItem
    {
        public searchObjType objType { get; set; }
        public string resultTitle { get; set; }
        public string resultSubTitle { get; set; }
        public productSmall productItem { get; set; }
        public string url { get; set; }
        public string photo { get; set; }
        public string cssClass
        {
            get
            {
                 
                switch (this.objType)
                {
                    case searchObjType.product:
                        return "product";


                    case searchObjType.staticPage:

                        return "staticPage";


                    case searchObjType.brand:

                        return "brand";

                    case searchObjType.model:

                        return "model";


                    default:

                        return null;

                }


            }
        }
    }

    public enum searchObjType
    {
        product = 1,
        staticPage = 2,
        brand = 3,
        model = 4
    }
}