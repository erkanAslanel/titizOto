using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace HelperAdmin
{
    public class DbWithControllerWithSortingWithFilter<T> : DbWithControllerWithSorting<T> where T : class,new()
    {
        public virtual List<T> getListWithFilter(int filterId)
        {
            return table.ToList();
        }

        public ActionResult IndexWithFilter(int filterId)
        {
            return View(getListWithFilter(filterId));
        }

        [HttpPost]
        public ActionResult moveUpWithFilter(int id, int filterId)
        {
            var item = getById(id);
            moveUpFilter(item, filterId);
            return View("Table", getListWithFilter(filterId));
        }

        [HttpPost]
        public ActionResult moveDownWithFilter(int id, int filterId)
        {
            var item = getById(id);
            moveDownFilter(item, filterId);
            return View("Table", getListWithFilter(filterId));
        }

        [HttpPost]
        public ActionResult setOrderAllWithFilter(FormCollection formCollection, int filterId)
        {
            int id = 0;
            int sequance = 0;
            string seqParameterName = "";
            List<string> idList = formCollection["selectedItem"].Split(',').Where(a => a != "false" && a != "true").ToList();
            foreach (var item in idList)
            {
                id = int.Parse(item);

                seqParameterName = "seq" + item.ToString();
                if (int.TryParse(formCollection[seqParameterName], out sequance))
                {
                    updateSequence(getById(id), sequance);
                }
            }

            return View("Table", getListWithFilter(filterId));
        }

        [HttpPost]
        public virtual ActionResult deleteWithFilter(int id, int filterId)
        {
            var item = getById(id);
            deleteItem(item);
            return View("Table", getListWithFilter(filterId));
        }

        public ActionResult updateIsDeletedWithFilter(int id, int filterId)
        {
            var item = getById(id);
            updateIsDeleted(id);
            return View("Table", getListWithFilter(filterId));
        }

        [HttpPost]
        public virtual ActionResult setFalseAllWithFilter(FormCollection formCollection, int filterId)
        {
            int id = 0;

            List<string> idList = formCollection["selectedItem"].Split(',').Where(a => a != "false" && a != "true").ToList();
            foreach (var item in idList)
            {
                id = int.Parse(item);
                changeStatu(getById(id), false);
            }


            return View("Table", getListWithFilter(filterId));
        }

        [HttpPost]
        public virtual ActionResult setTrueAllWithFilter(FormCollection formCollection, int filterId)
        {
            int id = 0;

            List<string> idList = formCollection["selectedItem"].Split(',').Where(a => a != "false" && a != "true").ToList();
            foreach (var item in idList)
            {
                id = int.Parse(item);
                changeStatu(getById(id), true);
            }

            return View("Table", getListWithFilter(filterId));
        }

        [HttpPost]
        public virtual ActionResult setDeleteAllWithFilter(FormCollection formCollection, int filterId)
        {
            int id = 0;

            List<string> idList = formCollection["selectedItem"].Split(',').Where(a => a != "false" && a != "true").ToList();
            foreach (var item in idList)
            {
                id = int.Parse(item);
                deleteItem(getById(id));
            }

            return View("Table", getListWithFilter(filterId));
        }

        public ActionResult GeneralSortingWithFilter(int filterId)
        {
            var list = getListWithFilter(filterId);

            if (list.Count == 0)
            {
                return RedirectToAction("IndexWithFilter");
            }
            else
            {
                return View(list);
            }
        }

        [HttpPost]
        public ActionResult GeneralSortingWithFilter(string sortArray)
        {
            return base.GeneralSorting(sortArray);
        }


        public IQueryable<T> orderColumnNameWithFilter(string ordering,int filterId)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, getListWithFilter(filterId).AsQueryable().Expression, Expression.Quote(orderByExp));
            return getListWithFilter(filterId).AsQueryable().Provider.CreateQuery<T>(resultExp);
        }

        public void moveUpFilter(T item,int filterId)
        {
            List<T> itemList = orderColumnNameWithFilter("sequence", filterId).ToList();

            int index = itemList.IndexOf(item);

            if (index > 0)
            {
                T item2 = itemList[index - 1];


                System.Reflection.PropertyInfo prop = this.objectType.GetProperty("sequence");
                int firstSeq = (int)prop.GetValue(item2, null);
                int secondSeq = (int)prop.GetValue(item, null);

                prop.SetValue(item2, secondSeq, null);
                prop.SetValue(item, firstSeq, null);

                db.Entry<T>(item).State = System.Data.EntityState.Modified;
                db.Entry<T>(item2).State = System.Data.EntityState.Modified;

                db.SaveChanges();
            }
        }

        public void moveDownFilter(T item,int filterId)
        {
            List<T> itemList = orderColumnNameWithFilter("sequence", filterId).ToList();

            int index = itemList.IndexOf(item);

            if (index < itemList.Count - 1)
            {
                T item2 = itemList[index + 1];

                System.Reflection.PropertyInfo prop = this.objectType.GetProperty("sequence");
                int firstSeq = (int)prop.GetValue(item2, null);
                int secondSeq = (int)prop.GetValue(item, null);

                prop.SetValue(item2, secondSeq, null);
                prop.SetValue(item, firstSeq, null);

                db.Entry<T>(item).State = System.Data.EntityState.Modified;
                db.Entry<T>(item2).State = System.Data.EntityState.Modified;

                db.SaveChanges();
            }
        }
    }
}