using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using titizOto.Models;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.ComponentModel;

namespace HelperAdmin
{
    public class DbWithController<T> : System.Web.Mvc.Controller, IBasicFunction, IAdminVariable, IRepository<T> where T : class,new()
    {
        protected DbSet<T> table;
        public titizOtoEntities db;
        private DbWithBasicFunction dbWithBasicFunction { get; set; }

        public DbWithController()
        {
            dbWithBasicFunction = new DbWithBasicFunction();
            db = dbWithBasicFunction.db;
            table = db.Set<T>();
        }

        #region baseMotod

        public virtual void add(T item)
        {
            table.Add(item);
            db.SaveChanges();
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void editItem(int id, T item)
        {
            db.Entry<T>(getById(id)).CurrentValues.SetValues(item);
            db.SaveChanges();

        }

        public virtual void deleteItem(T item)
        {
            table.Remove(item);
            db.SaveChanges();
        }

        public void updateItemIsDeleted(T item)
        {
            if (item != null)
            {
                System.Reflection.PropertyInfo prop = this.objectType.GetProperty("isDeleted");
                prop.SetValue(item, true, null);
                db.Entry<T>(item).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void changeStatu(Expression<Func<T, bool>> filter, bool changeToStatu)
        {
            List<T> items = table.Where(filter).ToList();

            if (items.Count == 0)
            {
                return;
            }

            foreach (var item in items)
            {
                changeStatu(item, changeToStatu);
            }

            db.SaveChanges();


        }

        public void changeStatu(T item, bool changeToStatu)
        {
            if (item != null)
            {
                System.Reflection.PropertyInfo prop = this.objectType.GetProperty("statu");
                prop.SetValue(item, changeToStatu, null);
                db.Entry<T>(item).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public T getBy(Expression<Func<T, bool>> filter)
        {
            return table.Where(filter).FirstOrDefault();
        }

        public List<T> getList(Expression<Func<T, bool>> filter)
        {
            return table.Where(filter).ToList();
        }

        public virtual List<T> getList()
        {
            return table.ToList();
        }

        public virtual List<T> getListWithoutSorting()
        {
            return table.ToList();
        }

        public List<T> getList(Expression<Func<T, int>> order)
        {
            return table.OrderBy(order).ToList();
        }

        public System.Web.Mvc.JsonResult getCategoryListData(List<tbl_category> categoryList, List<int> selectedIdList, bool isRootAdded, bool isRootSelected)
        {
            List<HierarchyData> dataList = new List<HierarchyData>();

            //Root gerçekten Seçiliyse, 
            if (selectedIdList != null && selectedIdList.Contains(0))
            {
                dataList.Add(new HierarchyData("Root", 0, null, true, "root"));
            }
            else // Root startup Control
            {
                if (isRootAdded)
                {
                    dataList.Add(new HierarchyData("Root", 0, null, isRootSelected, "root"));
                }
            }

            dataList.AddRange(recursiveCategoryList(0, categoryList, selectedIdList));

            return Json(dataList, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        public HtmlString jsonToHtmlString(System.Web.Mvc.JsonResult jsonData)
        {
            string data = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(jsonData.Data);
            return new HtmlString(data);
        }

        private List<HierarchyData> recursiveCategoryList(int parentId, List<tbl_category> categoryList, List<int> selectedIdlist)
        {
            List<HierarchyData> dataList = new List<HierarchyData>();

            List<tbl_category> categoryListFilter = categoryList.Where(a => a.parentId == parentId).OrderBy(a => a.sequence).ToList();

            foreach (tbl_category item in categoryListFilter)
            {
                HierarchyData dataItem = null;

                //Selected Data
                if (selectedIdlist != null && selectedIdlist.Contains(item.categoryId))
                {
                    //dataItem = new HierarchyData(item.name, item.categoryId, recursiveCategoryList(item.categoryId, categoryList, selectedIdlist), true, "lang" + item.langId.ToString());
                    dataItem = new HierarchyData(item.name, item.categoryId, recursiveCategoryList(item.categoryId, categoryList, selectedIdlist), true, "lang" + 1);
                }
                else
                {
                    //dataItem = new HierarchyData(item.name, item.categoryId, recursiveCategoryList(item.categoryId, categoryList, selectedIdlist), false, "lang" + item.langId.ToString());
                    dataItem = new HierarchyData(item.name, item.categoryId, recursiveCategoryList(item.categoryId, categoryList, selectedIdlist), false, "lang" + 1);

                }

                dataList.Add(dataItem);
            }

            return dataList;
        }

        public bool isUrlOk(string url, int id)
        {
            List<T> urlList = getList();
            bool val = true;
            string urlParm = "";
            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("url");

            if (id > 0) // Edit İşlemi
            {
                string currentUrl = (string)prop.GetValue(getById(id), null);
                foreach (var item in urlList)
                {
                    urlParm = (string)prop.GetValue(item, null);

                    //Edit Urlden Farklı bir Url girmiş olacak ve diğer urllere eşit bişey girerse
                    if (currentUrl != url && urlParm == url)
                    {
                        val = false;
                    }
                }
            }
            else // Create İşlemi
            {
                foreach (var item in urlList)
                {
                    urlParm = (string)prop.GetValue(item, null);

                    if (urlParm == url)
                    {
                        val = false;
                    }
                }
            }

            return val;

        }

        public string isUrlOkWithReturnVal(string title, int id)
        {
            string val = "";
            string url = createUrl(title);
            if (isUrlOk(url, id))
            {
                val = "border:1px solid #c1d779; background-color:#effeb9;";
            }
            else
            {
                val = "border:1px solid #e18b7c; background-color:#fccac1;";
            }

            return val + "|" + url;

        }

        public Type objectType
        {
            get
            {
                return typeof(T);

            }
        }

        public IQueryable<T> orderColumnName(string ordering)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);

            if (property==null)
            {
                return getListWithoutSorting().AsQueryable();
            }

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, getListWithoutSorting().AsQueryable().Expression, Expression.Quote(orderByExp));
            return getListWithoutSorting().AsQueryable().Provider.CreateQuery<T>(resultExp);
        }

        public IQueryable<T> orderColumnNameByDescending(string ordering)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);


            if (property == null)
            {
                return getListWithoutSorting().AsQueryable();
            }

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, getListWithoutSorting().AsQueryable().Expression, Expression.Quote(orderByExp));
            return getListWithoutSorting().AsQueryable().Provider.CreateQuery<T>(resultExp);
        }


        public T getById(int id, string idColumnName)
        {
            var type = typeof(T);

            var parameter = Expression.Parameter(type, "p");

            //p.ColumnName
            var left = Expression.Property(parameter, idColumnName);

            //p.ColumnName 7
            var right = Expression.Constant(id);

            //p.ColumnName == 7
            var filter = Expression.Equal(left, right);

            // p=> p.ColumnName ==7
            var firstOrDefaultMetod = Expression.Lambda(filter, parameter);

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "FirstOrDefault", new Type[] { type }, getListWithoutSorting().AsQueryable().Expression, Expression.Quote(firstOrDefaultMetod));

            return getListWithoutSorting().AsQueryable().Provider.Execute<T>(resultExp);
        }

        public T getById(int id)
        {
            return table.Find(id);
        }

        public virtual ActionResult Index()
        {
            return View(getList());
        }

        public virtual ActionResult Create()
        {
            return View(new T());
        }

        [HttpPost]
        public virtual ActionResult Create(T item)
        {

            if (Request.Form["imageclear"] != null)
            {
                ModelState.Clear();
                return View(item);
            }

            if (!isValidUrlSubmitCreate(item))
            {
                ModelState.AddModelError("url", "Seçtiğiniz adres kullanılmaktadır.");
            }

            if (!isValidUrlSubmitCreateEn(item))
            {
                ModelState.AddModelError("urlEn", "Seçtiğiniz adres kullanılmaktadır.(İngilizce)");
            }

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            try
            {
                add(item);
                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();
                cacheUpdate();
            }
            catch (Exception ex)
            {
                errorSend(ex, item.GetType().Name + " Ekleme İşlemi");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            return View(item);
        }

        public virtual ActionResult Edit(int id)
        {
            T item = getById(id);
            return View(item);
        }

        [HttpPost]
        public virtual ActionResult Edit(int id, T item)
        {

            if (Request.Form["imageclear"] != null)
            {
                ModelState.Clear();
                return View(item);
            }

            if (!isValidUrlSubmitEdit(item, id))
            {
                ModelState.AddModelError("url", "Seçtiğiniz adres kullanılmaktadır.");
            }

            if (!isValidUrlSubmitEditEn(item, id))
            {
                ModelState.AddModelError("urlEn", "Seçtiğiniz adres kullanılmaktadır.(İngilizce)");
            }

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            try
            {
                editItem(id, item);
                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();
                cacheUpdate();
            }
            catch (Exception ex)
            {
                errorSend(ex, item.GetType().Name + " Editleme İşlemi");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            return View(item);
        }

        [HttpPost]
        public virtual string UrlControl(string title, int id)
        {
            return isUrlOkWithReturnVal(title, id);
        }

        [HttpPost]
        public virtual ActionResult delete(int id)
        {
            var item = getById(id);
            deleteItem(item);
            cacheUpdate();
            return View("Table", getList());
        }

        public virtual ActionResult updateIsDeleted(int id)
        {
            var item = getById(id);
            updateItemIsDeleted(item);
            cacheUpdate();
            return View("Table", getList());
        }


        [HttpPost]
        public virtual ActionResult setFalseAll(FormCollection formCollection)
        {
            int id = 0;

            List<string> idList = formCollection["selectedItem"].Split(',').Where(a => a != "false" && a != "true").ToList();
            foreach (var item in idList)
            {
                id = int.Parse(item);
                changeStatu(getById(id), false);
            }

            cacheUpdate();
            return View("Table", getList());
        }

        [HttpPost]
        public virtual ActionResult setTrueAll(FormCollection formCollection)
        {
            int id = 0;

            List<string> idList = formCollection["selectedItem"].Split(',').Where(a => a != "false" && a != "true").ToList();
            foreach (var item in idList)
            {
                id = int.Parse(item);
                changeStatu(getById(id), true);
            }
            cacheUpdate();
            return View("Table", getList());
        }

        [HttpPost]
        public virtual ActionResult setDeleteAll(FormCollection formCollection)
        {
            int id = 0;

            List<string> idList = formCollection["selectedItem"].Split(',').Where(a => a != "false" && a != "true").ToList();
            foreach (var item in idList)
            {
                id = int.Parse(item);
                deleteItem(getById(id));
            }
            cacheUpdate();
            return View("Table", getList());
        }

        public bool isValidUrlSubmitCreate(T item)
        {
            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("url");

            if (prop != null)
            {
                string urlParm = (string)prop.GetValue(item, null);

                return isUrlOk(urlParm, 0);
            }
            else
            {
                return true;
            }

        }

        public bool isValidUrlSubmitEdit(T item, int id)
        {
            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("url");

            if (prop != null)
            {
                string urlParm = (string)prop.GetValue(item, null);

                return isUrlOk(urlParm, id);
            }
            else
            {
                return true;
            }

        }

        public virtual void cacheUpdate()
        {
            var objName = typeof(T).Name;
            var path = getCachePath(objName);

            if (!System.IO.File.Exists(path))
            {
                var fileCreater = System.IO.File.Create(path);
                fileCreater.Dispose();
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                file.WriteLine(DateTime.Now.ToString());
            }
        }

        public string getCachePath(string fileName)
        {
            return Server.MapPath("~/Download/cache/" + fileName + ".txt");

        }

 

        #endregion

        #region baseMotodRelatedLang

        [HttpPost]
        public virtual string UrlControlEn(string title, int id)
        {
            return isUrlOkWithReturnValEn(title, id);
        }

        public bool isValidUrlSubmitCreateEn(T item)
        {
            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("enUrl");

            if (prop != null)
            {
                string urlParm = (string)prop.GetValue(item, null);

                return isUrlOkEn(urlParm, 0);
            }
            else
            {
                return true;
            }

        }

        public bool isValidUrlSubmitEditEn(T item, int id)
        {
            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("enUrl");

            if (prop != null)
            {
                string urlParm = (string)prop.GetValue(item, null);

                return isUrlOkEn(urlParm, id);
            }
            else
            {
                return true;
            }

        }

        public bool isUrlOkEn(string url, int id)
        {
            List<T> urlList = getList();
            bool val = true;
            string urlParm = "";
            System.Reflection.PropertyInfo prop = this.objectType.GetProperty("enUrl");

            if (id > 0) // Edit İşlemi
            {
                string currentUrl = (string)prop.GetValue(getById(id), null);
                foreach (var item in urlList)
                {
                    urlParm = (string)prop.GetValue(item, null);

                    //Edit Urlden Farklı bir Url girmiş olacak ve diğer urllere eşit bişey girerse
                    if (currentUrl != url && urlParm == url)
                    {
                        val = false;
                    }
                }
            }
            else // Create İşlemi
            {
                foreach (var item in urlList)
                {
                    urlParm = (string)prop.GetValue(item, null);

                    if (urlParm == url)
                    {
                        val = false;
                    }
                }
            }

            return val;

        }

        public string isUrlOkWithReturnValEn(string title, int id)
        {
            string val = "";
            string url = createUrl(title);
            if (isUrlOkEn(url, id))
            {
                val = "border:1px solid #c1d779; background-color:#effeb9;";
            }
            else
            {
                val = "border:1px solid #e18b7c; background-color:#fccac1;";
            }

            return val + "|" + url;

        }


        #endregion

        #region generalFunction

        /// <summary>
        /// Warning, Information, Success, Failure
        /// </summary>
        public string getNotification(string notification, string type)
        {
            return ((IBasicFunction)dbWithBasicFunction).getNotification(notification, type);
        }

        /// <summary>
        /// Warning, Information, Success, Failure
        /// </summary>
        public string getNotification(string notification, string type, string classText)
        {
            return ((IBasicFunction)dbWithBasicFunction).getNotification(notification, type + " " + classText);
        }

        public string getNotificationDefaultSuccess()
        {
            return ((IBasicFunction)dbWithBasicFunction).getNotificationDefaultSuccess();
        }

        public string getNotificationDefaultError()
        {
            return ((IBasicFunction)dbWithBasicFunction).getNotificationDefaultError();
        }

        public string MD5(string text)
        {
            return ((IBasicFunction)dbWithBasicFunction).MD5(text);
        }

        public void errorSend(Exception ex, string msg)
        {
            ((IBasicFunction)dbWithBasicFunction).errorSend(ex, msg);
        }

        public string createUrl(string text)
        {
            return ((IBasicFunction)dbWithBasicFunction).createUrl(text);
        }

        #endregion

        #region generalFunctionWithMasterPage

        public AdminVariable getAdminVariable(int userId)
        {
            return ((IAdminVariable)dbWithBasicFunction).getAdminVariable(userId);
        }

        #endregion


        public string getSiteName(HttpRequestBase Request)
        {
            return ((IBasicFunction)dbWithBasicFunction).getSiteName(Request);
        }

        public string getSiteNameWithoutSlash(HttpRequestBase Request)
        {
            return ((IBasicFunction)dbWithBasicFunction).getSiteNameWithoutSlash(Request);
        }
    }
}