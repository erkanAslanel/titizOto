using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HelperSite;
using titizOto.Models;
using ViewModel.Basket;
using ViewModel.Shared;

namespace HelperSite.Shared
{
    public class basketShared : baseShared
    {
        public basketShared(titizOtoEntities db)
        {
            this.db = db;
        }

        public void updateStockCount(int basketId, int stockCount)
        {
            var item = db.tbl_basket.Where(a => a.basketId == basketId).FirstOrDefault();

            if (item != null)
            {
                item.quantity = stockCount;
                db.SaveChanges();
            }
        }

        public List<tbl_basket> getBasketTblByUserId(int userId)
        {
            return db.tbl_basket.Include("tbl_product").Where(a => a.userId == userId).ToList();
        }

        public List<tbl_basket> getBasketTblByGuest(string guestCode)
        {
            if (!string.IsNullOrWhiteSpace(guestCode))
            {
                return db.tbl_basket.Include("tbl_product").Where(a => a.guestCode == guestCode).ToList();
            }
            else
            {
                return null;
            }


        }

        public void deleteBasketById(int basketId)
        {
            var item = db.tbl_basket.Where(a => a.basketId == basketId).FirstOrDefault();

            if (item != null)
            {
                db.tbl_basket.Remove(item);
                db.SaveChanges();
            }

        }

        public List<Tuple<string, string>> getBasketOptionListByOptionStr(List<optionItem> list, string optionStr)
        {

            List<Tuple<string, string>> helperList = new List<Tuple<string, string>>();
            var optionStrList = optionStr.Split(',');

            foreach (var item in optionStrList)
            {
                var optionItem = list.Where(a => a.options.Any(b => b.Key == item)).FirstOrDefault();
                var optionHeader = optionItem.header;
                var optionText = optionItem.options.Where(a => a.Key == item).FirstOrDefault().Value;

                helperList.Add(new Tuple<string, string>(optionHeader, optionText));

            }

            return helperList;


        }

        public productAddResult addProductBasket(int productId, int userId, string guestCode, string optionValueList)
        {
            productShared pc = new productShared(db);

            var productItem = db.tbl_product.Include("tbl_stock").Include("tbl_productCritear.tbl_critear").Where(a => a.productId == productId).FirstOrDefault();

            if (!productItem.statu)
            {
                return productAddResult.statuError;
            }

            var optionList = pc.getOptionListByProductItem(productItem);

            if (optionList != null && optionList.Count > 0 && string.IsNullOrWhiteSpace(optionValueList))
            {
                return productAddResult.hasOption;
            }

            if (!productItem.tbl_stock.Any(a => a.stockCount > 0 && a.optionList == optionValueList))
            {
                return productAddResult.stockError;
            }

            var sameItemList = db.tbl_basket.Where(a => a.userId == userId && a.productId == productId).ToList().Where(a => a.guestCode == guestCode && a.optionList == optionValueList).FirstOrDefault();

            if (sameItemList != null)
            {
                var basketItemEdited = db.tbl_basket.Where(a => a.basketId == sameItemList.basketId).FirstOrDefault();
                basketItemEdited.quantity = basketItemEdited.quantity + 1;
                db.SaveChanges();
            }
            else
            {
                var basketItem = new tbl_basket();
                basketItem.createTime = DateTime.Now;
                basketItem.discountCode = null;
                basketItem.guestCode = guestCode;
                basketItem.optionList = optionValueList;
                basketItem.productId = productId;
                basketItem.userId = userId;
                basketItem.quantity = 1;

                db.tbl_basket.Add(basketItem);
                db.SaveChanges();
            }

            return productAddResult.added;
        }

        public string getOptionList(string optionListValue)
        {
            if (string.IsNullOrWhiteSpace(optionListValue))
            {
                return null;
            }


            List<string> list = optionListValue.Split(',').ToList();
            List<int> intList = new List<int>();

            int critearId = 0;
            foreach (var item in list)
            {
                if (int.TryParse(item, out critearId))
                {
                    intList.Add(critearId);
                }
            }

            intList.Sort();

            return string.Join(",", intList);

        }

        /// <summary>
        ///  List<basketItem> -- Basket Content  &  basketActionResult-- StockAdjust , Redirect ( Clear ) ...
        /// </summary>  
        public Tuple<List<basketItem>, basketActionResult> getBasketContent(topCart cartItem, int langId, string langCode, string mainPath, bool isDeletedInclude)
        {
            List<tbl_basket> list = new List<tbl_basket>();

            int userId = cartItem.userId;
            string guestCode = cartItem.guestGuid;

            if (cartItem.isRegisteredUser)
            {
                list = db.tbl_basket.Include("tbl_product.tbl_productCritear.tbl_critear").Include("tbl_product.tbl_gallery").Where(a => a.userId == userId).ToList();
            }
            else
            {
                list = db.tbl_basket.Include("tbl_product.tbl_productCritear.tbl_critear").Include("tbl_product.tbl_gallery").Where(a => a.guestCode == guestCode).ToList();
            }

            // Action && Basket Content
            return getBasketListByTblBasket(list, isDeletedInclude);
        }

        private Tuple<List<basketItem>, basketActionResult> getBasketListByTblBasket(List<tbl_basket> list,bool isDeletedInclude)
        {
            productShared pc = new productShared(db);

            basketActionResult basketAction = basketActionResult.success;
            List<basketItem> helperList = new List<basketItem>();

            foreach (var item in list)
            {
                basketItem helperItem = new basketItem();
                var productItem = item.tbl_product;

                // delete Basket
                if (productItem == null || productItem.statu == false || productItem.isDeleted)
                {
                    deleteBasketById(item.basketId);
                    return new Tuple<List<basketItem>, basketActionResult>(null, basketActionResult.redirect);

                }

                if (item.quantity < 1)
                {
                    deleteBasketById(item.basketId);
                    return new Tuple<List<basketItem>, basketActionResult>(null, basketActionResult.redirect);
                }


                //option
                var optionList = pc.getOptionListByProductItem(productItem);
                if (optionList != null && optionList.Count > 0)
                {
                    // delete Basket
                    if (!pc.isProductOptionValid(optionList, item.optionList, isDeletedInclude))
                    {
                        deleteBasketById(item.basketId);
                        return new Tuple<List<basketItem>, basketActionResult>(null, basketActionResult.redirect);
                    }

                    // Renk Seçimi Metalik
                    helperItem.optionItemList = getBasketOptionListByOptionStr(optionList, item.optionList);
                    helperItem.optionCode = item.optionList;
                }

                // stock
                int stockCount = pc.getProductStockAvailableCount(item.productId, item.optionList);
                if (stockCount < 1)
                {
                    deleteBasketById(item.basketId);
                    return new Tuple<List<basketItem>, basketActionResult>(null, basketActionResult.redirect);
                }

                if (stockCount < item.quantity)
                {
                    helperItem.quantity = stockCount;
                    updateStockCount(item.basketId, stockCount);
                    basketAction = basketActionResult.stockAdjust;
                }
                else
                {
                    helperItem.quantity = item.quantity;
                }

                // name
                helperItem.description = productItem.name;

                // price
                helperItem.productPriceDec = pc.calcPriceProduct(productItem);
                helperItem.productTotalPriceDec = helperItem.productPriceDec * helperItem.quantity;

                // image  
                var photoItem = pc.getProductGallery(productItem, "110", "74").Where(a => a.Item2 == item.optionList).FirstOrDefault();
                if (photoItem != null)
                {
                    helperItem.photo = photoItem.Item1;
                }


                // discount 
                helperItem.discountCode = item.discountCode;

                // basketId 
                helperItem.basketId = item.basketId;

                // productId 
                helperItem.productId = item.productId;

                helperList.Add(helperItem);

            }

            return new Tuple<List<basketItem>, basketActionResult>(helperList, basketAction);
        }

        public Tuple<helperBasket, basketActionResult> getBasketHelperWithProductAndDiscount(topCart cartItem, int langId, string langCode, string mainPath, bool isDeletedInclude)
        {
            discountShared ds = new discountShared(db);
            helperBasket helperPage = new helperBasket();

            var basketContentItem = getBasketContent(cartItem, langId, langCode, mainPath, isDeletedInclude);

            helperPage.basketList = basketContentItem.Item1;
            helperPage.actionMsg = basketContentItem.Item2;

            if (!helperPage.isBasketValid)
            {
                return new Tuple<helperBasket, basketActionResult>(null, basketActionResult.redirect);
            }

            // Discount Calculate
            var discountObject = ds.getDiscountSummary(basketContentItem.Item1, cartItem.userId);
            if (discountObject.Item2)
            {
                helperPage.discountList = discountObject.Item1;
                helperPage.isDiscountValid = discountObject.Item2;
            }

            if (!helperPage.isDiscountValid)
            {
                return new Tuple<helperBasket, basketActionResult>(null, basketActionResult.redirect);
            }

            helperPage.calculateSum();

            return new Tuple<helperBasket, basketActionResult>(helperPage, basketActionResult.success);
        }
         
        public void updateUserCartFromGuestCode(int userId, string guestCode)
        {
            var basketList = db.tbl_basket.Where(a => a.guestCode == guestCode).AsEnumerable();

            foreach (var item in basketList)
            { 
                var existingItem = db.tbl_basket.Where(a => a.userId == userId).ToList().Where(a => a.productId == item.productId && a.optionList == item.optionList).FirstOrDefault();

                if (existingItem != null)
                {
                    var updateItem = db.tbl_basket.Where(a => a.basketId == existingItem.basketId).FirstOrDefault();
                    updateItem.quantity = updateItem.quantity + item.quantity;
                    db.tbl_basket.Remove(item);
                }
                else
                {
                    item.userId = userId;
                } 
            }

            db.SaveChanges(); 
        }
    }

    public enum productAddResult
    {
        added,
        hasOption,
        stockError,
        statuError

    }
}