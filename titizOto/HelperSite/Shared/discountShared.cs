using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelperAdmin;
using titizOto.Models;
using ViewModel.Basket;

namespace HelperSite.Shared
{
    public class discountShared : baseShared
    {
        public discountShared(titizOtoEntities db)
        {
            this.db = db;
        }

        public bool isDiscountExist(List<basketItem> basketContent)
        {
            return basketContent.Any(a => !string.IsNullOrWhiteSpace(a.discountCode));
        }

        public void updateAllBasketItemDiscountSame(List<basketItem> basketContent)
        {
            var discountString = basketContent.Max(a => a.discountCode);

            var list = basketContent.Where(a => a.discountCode != discountString).ToList();

            foreach (var item in list)
            {
                updateBasketItemDiscount(item.basketId, discountString);
            }

        }

        public void deleteDiscountOnBasketByDiscountId(List<basketItem> basketContent, int discountId)
        {
            var discountItem = db.tbl_discount.Where(a => a.discountId == discountId).FirstOrDefault();

            if (discountItem != null)
            {
                string newCode = "";
                List<string> discountCodeList = new List<string>();

                foreach (var item in basketContent)
                {
                    discountCodeList = item.discountCode.Split(',').ToList();

                    if (discountCodeList.Contains(discountItem.code.Trim()))
                    {
                        discountCodeList.Remove(discountItem.code.Trim());

                        newCode = string.Join(",", discountCodeList);

                        updateBasketItemDiscount(item.basketId, newCode);
                    }

                }
            }
        }

        public void addDiscountBasket(List<basketItem> basketContent, string discountCode)
        {
            var list = new List<string>();
            string editedCode = "";

            foreach (var item in basketContent)
            {
                if (!string.IsNullOrWhiteSpace(item.discountCode))
                {
                    list = item.discountCode.Split(',').ToList();
                    list.Add(discountCode);
                    editedCode = string.Join(",", list);

                    updateBasketItemDiscount(item.basketId, editedCode);

                }
                else
                {
                    updateBasketItemDiscount(item.basketId, discountCode);
                }
            }


        }

        public Tuple<bool, tbl_discount, discountErrorType> isDiscountValidForBasketContent(List<basketItem> basketContent, string discountCode, int? userId)
        {
            var dicountItem = db.tbl_discount.Where(a => a.code == discountCode && a.statu).FirstOrDefault();

            if (dicountItem == null)
            {
                return new Tuple<bool, tbl_discount, discountErrorType>(false, null, discountErrorType.notFoundInSystem);
            }

            // Product
            if (!isValidProductConstraint(basketContent, dicountItem))
            {
                return new Tuple<bool, tbl_discount, discountErrorType>(false, null, discountErrorType.productError);
            }

            // Rep Time
            if (!isValidRepConstraint(basketContent, dicountItem))
            {
                return new Tuple<bool, tbl_discount, discountErrorType>(false, null, discountErrorType.repTime);
            }

            // Min Spent Basket
            if (!isValidMinimumSpent(basketContent, dicountItem))
            {
                return new Tuple<bool, tbl_discount, discountErrorType>(false, null, discountErrorType.minSpent);
            }

            // Min Count Basket
            if (!isValidBasketCount(basketContent, dicountItem))
            {
                return new Tuple<bool, tbl_discount, discountErrorType>(false, null, discountErrorType.minCount);
            }

            // Date 
            if (!isDateValid(dicountItem))
            {
                return new Tuple<bool, tbl_discount, discountErrorType>(false, null, discountErrorType.date);
            }

            // User
            if (!isUserValid(dicountItem, userId))
            {
                return new Tuple<bool, tbl_discount, discountErrorType>(false, null, discountErrorType.user);
            }

            return new Tuple<bool, tbl_discount, discountErrorType>(true, dicountItem, discountErrorType.none);
        }

        public void updateBasketItemDiscount(int basketId, string discountCode)
        {
            var item = db.tbl_basket.Where(a => a.basketId == basketId).FirstOrDefault();

            if (item != null)
            {
                item.discountCode = discountCode;
                if (string.IsNullOrWhiteSpace(discountCode))
                {
                    item.discountCode = null;
                }


                db.SaveChanges();
            }

        }

        public List<int> getProductIdListByDiscountItem(tbl_discount item)
        {

            if (!string.IsNullOrWhiteSpace(item.productList))
            {
                var strProductIdList = item.productList.Split(',').ToList();
                var productIdList = strProductIdList.Select(int.Parse).ToList();

                if (!string.IsNullOrWhiteSpace(item.exculudeProductList))
                {
                    var strExcludeProductIdList = item.exculudeProductList.Split(',').ToList();
                    var excludeProductIdList = strExcludeProductIdList.Select(int.Parse).ToList();

                    List<int> filterList = new List<int>();

                    foreach (var productId in productIdList)
                    {
                        if (!excludeProductIdList.Contains(productId))
                        {
                            filterList.Add(productId);
                        }
                    }

                    return filterList;
                }
                else
                {
                    return productIdList;
                }
            }
            else
            {
                var productIdList = db.tbl_product.Select(a => a.productId).ToList();

                if (!string.IsNullOrWhiteSpace(item.exculudeProductList))
                {
                    var strExcludeProductIdList = item.exculudeProductList.Split(',').ToList();
                    var excludeProductIdList = strExcludeProductIdList.Select(int.Parse).ToList();

                    List<int> filterList = new List<int>();

                    foreach (var productId in productIdList)
                    {
                        if (!excludeProductIdList.Contains(productId))
                        {
                            filterList.Add(productId);
                        }
                    }

                    return filterList;
                }
                else
                {
                    return productIdList;
                }
            }


        }

        public void deleteDiscountCodeOnBasket(List<basketItem> basketContent, string discountCode)
        {
            string discountText = basketContent.FirstOrDefault().discountCode;

            var discountList = discountText.Split(',').ToList();

            if (discountList.Contains(discountCode))
            {
                discountList.Remove(discountCode);
            }

            var updatedText = string.Join(",", discountList);

            if (string.IsNullOrWhiteSpace(updatedText))
            {
                updatedText = null;
            }


            foreach (var item in basketContent)
            {
                var basketItem = db.tbl_basket.Where(a => a.basketId == item.basketId).FirstOrDefault();

                if (basketItem != null)
                {
                    basketItem.discountCode = updatedText;
                    db.SaveChanges();
                }
            }


        }

        #region Constraint Check

        public bool isValidProductConstraint(List<basketItem> basketContent, tbl_discount item)
        {
            if (string.IsNullOrWhiteSpace(item.productList) && string.IsNullOrWhiteSpace(item.exculudeProductList))
            {
                return true;
            }

            var productIdList = getProductIdListByDiscountItem(item);

            var discountTypeItem = ((discountType)item.typeId);


            if (discountTypeItem == discountType.basketAmount || discountTypeItem == discountType.basketPercent)
            {
                return basketContent.Select(a => a.productId).All(a => productIdList.Contains(a));
            }

            if (discountTypeItem == discountType.productAmount || discountTypeItem == discountType.productPercent)
            {
                return basketContent.Select(a => a.productId).Any(a => productIdList.Contains(a));
            }

            return false;
        }

        public bool isValidRepConstraint(List<basketItem> basketContent, tbl_discount item)
        {
            // ToDo : Discount Rep With Order 
            return true;

        }

        public bool isValidMinimumSpent(List<basketItem> basketContent, tbl_discount item)
        {
            var totalSpent = basketContent.Sum(a => a.productTotalPriceDec);
            return (totalSpent > item.minBasketAmount);
        }

        public bool isValidBasketCount(List<basketItem> basketContent, tbl_discount item)
        {
            var totalCount = basketContent.Sum(a => a.quantity);
            return (totalCount > item.minBasketCount);

        }

        public bool isDateValid(tbl_discount item)
        {
            if (item.startDate <= DateTime.Now && DateTime.Now <= item.endDate)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool isUserValid(tbl_discount item, int? userId)
        {
            if (item.userId == 0)
            {
                return true;
            }
            else
            {
                if (userId.HasValue && item.userId == userId.Value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


        }

        public bool isAllreadyUse(List<basketItem> basketContent, tbl_discount item)
        {
            return basketContent.Any(a => a.discountCode != null && a.discountCode.Split(',').ToList().Contains(item.code.Trim()));
        }

        #endregion


        private List<discountItem> getDiscountSummaryByDiscountItemList(List<tbl_discount> discountList, List<basketItem> basketContent)
        {
            List<discountItem> helperList = new List<discountItem>();

            foreach (var item in discountList)
            {
                helperList.Add(getDiscountSummaryByDiscountItem(item, basketContent));
            }

            return helperList;
        }

        private discountItem getDiscountSummaryByDiscountItem(tbl_discount discountItem, List<basketItem> basketContent)
        {
            discountItem helperItem = new discountItem();

            helperItem.name = discountItem.code;
            helperItem.description = discountItem.description;
            helperItem.discountId = discountItem.discountId;

            List<int> productIdList = new List<int>();
            decimal discountAmount = 0;
            decimal totalPrice = basketContent.Sum(a => a.productTotalPriceDec);

            switch (((discountType)discountItem.typeId))
            {
                case discountType.basketPercent:

                    discountAmount = totalPrice * (discountItem.amountPercent / 100);

                    break;
                case discountType.basketAmount:

                    discountAmount = discountItem.amountPercent;
                    break;

                case discountType.productPercent:
                     
                    productIdList = getProductIdListByDiscountItem(discountItem);

                    foreach (var item in basketContent)
                    {
                        if (productIdList.Contains(item.productId))
                        {
                            discountAmount = discountAmount + (item.productTotalPriceDec * (discountItem.amountPercent / 100));
                        }
                    }  

                    break;

                case discountType.productAmount:

                    productIdList = getProductIdListByDiscountItem(discountItem);

                    foreach (var item in basketContent)
                    {
                        if (productIdList.Contains(item.productId))
                        {
                            discountAmount = discountAmount + item.productTotalPriceDec;
                        }
                    }
                    break;

            }

            helperItem.discountAmount = Math.Round(discountAmount, 2, MidpointRounding.AwayFromZero);

            return helperItem;

        }

        public Tuple<List<discountItem>, bool> getDiscountSummary(List<basketItem> basketContent, int userId)
        { 
            List<discountItem> discountList = new List<discountItem>();

            if (isDiscountExist(basketContent))
            {
                updateAllBasketItemDiscountSame(basketContent);
                string discountText = basketContent.FirstOrDefault().discountCode;
                var discountTextList = discountText.Split(',').ToList();

                int? discountForUserId = null;
                if (userId != 0)
                {
                    discountForUserId = userId;
                }

                List<tbl_discount> discountListTable = new List<tbl_discount>();

                foreach (var item in discountTextList)
                {
                    var dicountResult = isDiscountValidForBasketContent(basketContent, item, discountForUserId);

                    if (dicountResult.Item1)
                    {
                        discountListTable.Add(dicountResult.Item2);
                    }
                    else
                    {
                        // Return Index
                        deleteDiscountCodeOnBasket(basketContent, item);
                        return new Tuple<List<discountItem>, bool>(discountList, false);

                    }
                }

                discountList = getDiscountSummaryByDiscountItemList(discountListTable, basketContent);

              
            }

            return new Tuple<List<discountItem>, bool>(discountList, true); 
        }

    }

    public enum discountType
    {
        basketPercent = 2,
        basketAmount = 1,
        productPercent = 4,
        productAmount = 3,

    }

    public enum discountErrorType
    {
        none,
        notFoundInSystem,
        productError,
        repTime,
        minSpent,
        minCount,
        date,
        user,
        unExpected,
        success,
        alreadyInUse

    }
}