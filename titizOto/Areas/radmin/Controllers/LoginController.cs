using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;

namespace titizOto.Areas.radmin.Controllers
{
    public class LoginController : Controller
    {
        DbWithBasicFunction dbItem = new DbWithBasicFunction();

        public ActionResult Index()
        {
            if (Session["adminId"] != null && Session["roleId"] != null)
            {
                Response.Redirect("~/radmin/Dashboard/Index");
                return null;
            }
            else
            {
                if (Request.Cookies["adminCookie"] != null && Request.Cookies["adminCookie"]["userHashVal"] != null && Request.Cookies["adminCookie"]["userHashValTwo"] != null)
                {
                    //hashed Email
                    string hashValOne = Request.Cookies["adminCookie"]["userHashVal"];
                    //hashed Password
                    string hashValTwo = Request.Cookies["adminCookie"]["userHashValTwo"];

                    var list = dbItem.db.tbl_adminUser.ToList();

                    foreach (var item in list)
                    {
                        if (dbItem.MD5(item.email).Substring(0, 7) == hashValOne && item.password.Substring(0, 7).ToLower() == hashValTwo)
                        {
                            Session["adminId"] = item.userId;
                            Session["roleId"] = item.adminRoleId;

                            return RedirectToAction("Index", "Dashboard");
                        }
                    }

                 
                    Response.Cookies["adminCookie"].Expires = DateTime.Now.AddDays(-1);  
                }

                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(FormCollection formList)
        {
            bool chbox = false;
            string email = formList["login"];
            string password = formList["password"];
            string hashedPassword = dbItem.MD5(password).ToLower();

            if (formList["chbox"] != null)
            {
                chbox = true;
            }

            //Boş giriş
            if (email == "" || password == "")
            {
                ViewBag.html = dbItem.getNotification("Kullanıcı adı / Şifre boş geçilemez", "Failure", "mt0");
            }

            try
            {
                var adminUserItem = dbItem.db.tbl_adminUser.Where(a => a.email == email && a.password == hashedPassword).FirstOrDefault();

                if (adminUserItem != null)
                {
                    Session["adminId"] = adminUserItem.userId;
                    Session["roleId"] = adminUserItem.adminRoleId;
                    ViewBag.html = dbItem.getNotification("Başarıyla giriş yaptınız yönlendiriliyorsunuz..", "Success", "mt0");

                    if (chbox)
                    {
                        HttpCookie myCookie = new HttpCookie("adminCookie");
                        myCookie["userHashVal"] = dbItem.MD5(adminUserItem.email).Substring(0, 7);
                        myCookie["userHashValTwo"] = hashedPassword.Substring(0, 7);
                        myCookie.Expires = DateTime.Now.AddMonths(9);
                        Response.Cookies.Add(myCookie);
                    }
                }
                else
                {
                    ViewBag.html = dbItem.getNotification("Kullanıcı adı / Şifre hatalı giriş.", "Failure", "mt0");
                }

            }
            catch (Exception ex)
            {
                dbItem.errorSend(ex, "Admin ekranı giriş işleminde");
                ViewBag.html = dbItem.getNotification("Giriş işleminde hata meydana gelmiştir.", "Failure", "mt0");
            }

            return View(ViewBag);
        }
    }
}
