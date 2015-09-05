using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelperAdmin
{
    public class DbWithControllerWithSorting<T> : DbWithController<T>, ISortable<T> where T : class,new()
    {
        public void updateSequence(T item, int sequence)
        {
            if (item != null)
            {
                System.Reflection.PropertyInfo prop = this.objectType.GetProperty("sequence");
                prop.SetValue(item, sequence, null);
                db.Entry<T>(item).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void updateSequence(int itemId, int sequence)
        {
            var item = getById(itemId);
            if (item != null)
            {
                System.Reflection.PropertyInfo prop = this.objectType.GetProperty("sequence");
                prop.SetValue(item, sequence, null);
                db.Entry<T>(item).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void updateSequence(List<T> items, int sequence)
        {
            foreach (var item in items)
            {
                updateSequence(item, sequence);
            }

            db.SaveChanges();
        }

        public void moveUp(T item)
        {
            List<T> itemList = orderColumnName("sequence").ToList();

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

        public void moveDown(T item)
        {
            List<T> itemList = orderColumnName("sequence").ToList();

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

        public override List<T> getList()
        {
            return orderColumnName("sequence").ToList();
        }

        public override void add(T item)
        {
            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("sequence");
            if (prop != null)
            {
                prop.SetValue(item, getLastSequence(), null);
            }


            base.add(item);
        }

        public override ActionResult Edit(int id, T item)
        {
            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("sequence");
            if (prop != null)
            {
                prop.SetValue(item, prop.GetValue(getById(id), null), null);
            }

            return base.Edit(id, item);
        }

        public int getLastSequence()
        {
            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("sequence");

            List<T> itemList = orderColumnName("sequence").ToList();
            T item = itemList.LastOrDefault();

            if (item == null)
            {
                return 1;
            }
            else
            {
                return (int)prop.GetValue(item, null) + 1;
            }
        }

        [HttpPost]
        public virtual ActionResult moveUp(int id)
        {
            var item = getById(id);
            moveUp(item);
            cacheUpdate();
            return View("Table", getList());
        }

        [HttpPost]
        public virtual ActionResult moveDown(int id)
        {
            var item = getById(id);
            moveDown(item);
            cacheUpdate();
            return View("Table", getList());
        }

        [HttpPost]
        public virtual ActionResult setOrderAll(FormCollection formCollection)
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
            cacheUpdate();
            return View("Table", getList());
        }

        public virtual ActionResult GeneralSorting()
        {
            var list = getList();

            if (list.Count == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(list);
            }

        }


        [HttpPost]
        public virtual ActionResult GeneralSorting(string sortArray)
        {
            if (string.IsNullOrWhiteSpace(sortArray))
            {
                ViewBag.success = false;
                ViewBag.resultHtml = getNotification("Sıralama Kayıtları Alınamadı", "Information");

                return View(getList());
            }

            System.Collections.Generic.List<defaultSorting> results = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<defaultSorting>>(sortArray);


            int sequence = 1;

            try
            {

                foreach (var item in results)
                {
                    updateSequence(item.dataId, sequence);
                    sequence++;
                }
                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();
                cacheUpdate();
                return View(getList());
            }
            catch (Exception ex)
            {
                errorSend(ex, "General Sorting İşlemi");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
                return View(getList());
            }
        }
    }
}