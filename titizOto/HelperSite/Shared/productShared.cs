using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HelperSite;
using titizOto.Models;
using ViewModel.Shared;


namespace HelperSite.Shared
{
    public class productShared : baseShared
    {
        public productShared(titizOtoEntities db)
        {
            this.db = db;
        }

        public decimal calcPriceProduct(tbl_product item)
        {
            return Math.Round(calcPriceProduct(item.price, item.isTaxInclude, item.taxPercent, item.discountPrice, item.isDiscountPriceActive), 2, MidpointRounding.AwayFromZero);
        }

        public decimal calcPriceProduct(decimal price, bool isTaxInclude, int taxPercent, decimal discountPrice, bool isDiscountPrice)
        {
            decimal val = 0;

            if (isDiscountPrice)
            {
                if (isTaxInclude)
                {
                    val = discountPrice;
                }
                else
                {
                    val = (discountPrice / 100) * (100 + taxPercent);
                }
            }
            else
            {
                if (isTaxInclude)
                {

                    val = price;
                }
                else
                {
                    val = (price / 100) * (100 + taxPercent);
                }

            }

            return val;

        }

        public List<productSmall> getProductSummary(string productUrl, List<tbl_product> list, string langCulture, string langCode)
        {
            return getProductSummary(productUrl, list, langCulture, langCode, "180", "125");

        }

        public List<productSmall> getProductSummary(string productUrl, List<tbl_product> list, string langCulture, string langCode, string photoWidth, string photoHeight)
        {
            List<productSmall> helperList = new List<productSmall>();


            var currentCultureInfo = CultureInfo.CreateSpecificCulture(langCulture);

            foreach (var item in list)
            {
                productSmall helperItem = new productSmall();

                // Car Brand , Car Model  Validation
                if (item.tbl_carModelProduct == null || item.tbl_carModelProduct.Where(a => a.tbl_carModel != null).FirstOrDefault() == null || item.tbl_carModelProduct.Where(a => a.tbl_carModel != null).FirstOrDefault().tbl_carModel == null || item.tbl_carModelProduct.Where(a => a.tbl_carModel != null).FirstOrDefault().tbl_carModel.tbl_carBrand == null)
                {
                    continue;
                }

                helperItem = getProductSummaryOneItem(productUrl, item, currentCultureInfo, langCode, photoWidth, photoHeight);
                helperList.Add(helperItem);
            }



            return helperList;

        }

        public productSmall getProductSummaryOneItem(string productUrl, tbl_product item, CultureInfo currentCultureInfo, string langCode, string photoWidth, string photoHeight)
        {

            productSmall helperItem = new productSmall();

            // Car Brand , Car Model  Validation
            if (item.tbl_carModelProduct == null || item.tbl_carModelProduct.Where(a => a.tbl_carModel != null).FirstOrDefault() == null || item.tbl_carModelProduct.Where(a => a.tbl_carModel != null).FirstOrDefault().tbl_carModel == null || item.tbl_carModelProduct.Where(a => a.tbl_carModel != null).FirstOrDefault().tbl_carModel.tbl_carBrand == null)
            {
                return null;
            }


            helperItem.brand = item.tbl_carModelProduct.Where(a => a.tbl_carModel != null).FirstOrDefault().tbl_carModel.tbl_carBrand.name;
            helperItem.currency = "TL";
            helperItem.model = item.tbl_carModelProduct.Where(a => a.tbl_carModel != null).FirstOrDefault().tbl_carModel.name;
            helperItem.name = item.name;
            helperItem.shortDescription = item.shortDescription;
            helperItem.width = photoWidth;
            helperItem.height = photoHeight;

            if (item.tbl_gallery == null || item.tbl_gallery.Where(a => a.statu).Count() == 0)
            {
                helperItem.photo = "ImageShow/Resize/Gallery/" + "noPhoto.png" + "/" + photoWidth + "/" + photoHeight;
            }
            else
            {
                helperItem.photo = "ImageShow/ShowGuid/gallery/" + item.tbl_gallery.Where(a => a.statu).OrderBy(a => a.sequence).FirstOrDefault().guid + "/" + photoWidth + "/" + photoHeight;

            }

            decimal price = calcPriceProduct(item);
            helperItem.price = price.ToString("F2", currentCultureInfo);

            helperItem.url = langCode + "/" + productUrl + "/" + item.tbl_carModelProduct.FirstOrDefault().tbl_carModel.tbl_carBrand.url + "/" + item.tbl_carModelProduct.Where(a => a.tbl_carModel != null).FirstOrDefault().tbl_carModel.url + "/" + item.url + ".html";
            helperItem.productId = item.productId;

            var taxItem = getProductWithoutTaxPriceAndTaxPrice(item, currentCultureInfo);
            helperItem.withoutTaxPrice = taxItem.Item2;

            return helperItem;

        }

        public List<optionItem> getOptionListByProductItem(tbl_product productItem)
        {

            List<optionItem> helper = new List<optionItem>();
            List<tbl_critear> parentCritearList = new List<tbl_critear>();
            var optionList = productItem.tbl_productCritear.Select(a => a.tbl_critear).Where(a => a != null && a.statu).ToList();
            var optionListId = optionList.Select(a => a.critearId).ToList();

            foreach (var item in optionList)
            {
                var parentCritearItem = db.tbl_critear.Where(a => a.critearId == item.parentId && a.statu).FirstOrDefault();

                if (parentCritearItem != null)
                {
                    parentCritearList.Add(parentCritearItem);
                }
            }

            var distictParentItem = parentCritearList.Distinct().ToList();

            foreach (var item in distictParentItem)
            {
                optionItem helperItem = new optionItem();

                helperItem.header = item.name;
                helperItem.headerId = item.critearId.ToString();
                var subList = db.tbl_critear.Where(a => a.parentId == item.critearId && optionListId.Contains(a.critearId)).OrderBy(a => a.sequence).ToList();

                if (subList != null)
                {
                    helperItem.options = new List<KeyValuePair<string, string>>();
                }

                foreach (var subItem in subList)
                {
                    helperItem.options.Add(new KeyValuePair<string, string>(subItem.critearId.ToString(), subItem.name));
                }

                helper.Add(helperItem);

            }

            return helper;

        }

        public paging getPageItem(int totalItem, int currentPage, int pagePerItem, string pageSuffix)
        {
            paging item = new paging();

            if (totalItem > pagePerItem)
            {
                item.isPagingExist = true;
            }

            item.currentPage = currentPage;
            item.totalItems = totalItem;
            item.itemsPerPage = pagePerItem;
            item.pageSufix = pageSuffix;

            return item;
        }

        public int getCurrentPage(HttpRequestBase Request, int productCount, int pagePerItem)
        {
            int currentPage = 1;
            if (Request.QueryString["sayfa"] != null)
            {
                if (int.TryParse(Request.QueryString["sayfa"], out currentPage))
                {
                    if (currentPage < 1)
                    {
                        currentPage = 1;
                    }

                    int totalPage = productCount / pagePerItem;

                    if (productCount % pagePerItem != 0)
                    {
                        totalPage = totalPage + 1;
                    }

                    if (totalPage < currentPage)
                    {
                        currentPage = totalPage;
                    }
                }
                else
                {
                    currentPage = 1;
                }
            }

            return currentPage;
        }

        public int getProductStockAvailableCount(int productId, string optionList)
        {

            if (string.IsNullOrWhiteSpace(optionList))
            {
                optionList = null;
            }

            var list = db.tbl_stock.Where(a => a.productId == productId).ToList();
            return list.Where(a => a.optionList == optionList).Select(a => a.stockCount).FirstOrDefault();
        }

        public bool isProductHasOption(List<optionItem> list, int critearId)
        {
            var critearStr = critearId.ToString();

            if (list != null && list.Count > 0 && list.Any(a => a.options.Any(b => b.Key == critearStr)))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool isProductOptionValid(List<optionItem> list, string optionList, bool isDeletedInclude)
        {

            var optionListStr = optionList.Split(',');

            var returnVal = true;
            int critearId = 0;

            foreach (var item in optionListStr)
            {
                if (int.TryParse(item, out critearId))
                {
                    tbl_critear critearItem = null;

                    if (!isDeletedInclude)
                    {
                        critearItem = db.tbl_critear.Where(a => a.critearId == critearId && a.statu).FirstOrDefault();
                    }
                    //else
                    //{
                    //     = db.tbl_critear.Where(a => a.critearId == critearId && a.statu && a.isDeleted==false).FirstOrDefault();
                    //}

                    if (critearItem != null)
                    {
                        if (!isProductHasOption(list, critearId))
                        {
                            returnVal = false;
                            break;
                        }
                    }
                    else
                    {
                        returnVal = false;
                        break;
                    }

                }
                else
                {
                    returnVal = false;
                    break;
                }
            }



            return returnVal;
        }

        public List<Tuple<string, string>> getProductGallery(tbl_product productItem, string photoWidth, string photoHeight)
        {
            List<Tuple<string, string>> helperList = new List<Tuple<string, string>>();

            if (productItem.tbl_gallery == null || productItem.tbl_gallery.Where(a => a.statu).Count() == 0)
            {
                // empty Photo With Null Option
                var emtyPhoto = "ImageShow/Resize/Gallery/" + "noPhoto.png" + "/" + photoWidth + "/" + photoHeight;
                helperList.Add(new Tuple<string, string>(emtyPhoto, null));
            }
            else
            {
                var galleryList = productItem.tbl_gallery.Where(a => a.statu).OrderBy(a => a.sequence).ToList();
                string photo = "";

                foreach (var item in galleryList)
                {
                    photo = "ImageShow/ShowGuid/gallery/" + item.guid + "/" + photoWidth + "/" + photoHeight;
                    helperList.Add(new Tuple<string, string>(photo, item.optionList));
                }
            }

            return helperList;

        }

        /// <summary>
        /// withoutTaxPrice,withoutTaxPriceString,taxPrice,taxPriceString
        /// </summary>
        /// <param name="productItem"></param>
        /// <returns></returns>
        public Tuple<decimal, string, decimal, string> getProductWithoutTaxPriceAndTaxPrice(tbl_product productItem, CultureInfo priceFormat)
        {
            decimal withoutTaxPrice = 0;
            string withoutTaxPriceStr = "";
            decimal taxPrice = 0;
            string taxPriceStr = "";

            if (productItem.isTaxInclude)
            {
                withoutTaxPrice = (productItem.price / (100 + productItem.taxPercent)) * 100;
                taxPrice = productItem.price - withoutTaxPrice;

            }
            else
            {
                withoutTaxPrice = productItem.price;
                taxPrice = (withoutTaxPrice / 100) * productItem.taxPercent;

            }

            withoutTaxPriceStr = withoutTaxPrice.ToString("N2", priceFormat);
            taxPriceStr = taxPrice.ToString("N2", priceFormat);

            return new Tuple<decimal, string, decimal, string>(withoutTaxPrice, withoutTaxPriceStr, taxPrice, taxPriceStr);
        }
    }


}