using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;
using titizOto.Models;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class UserController : DbWithController<tbl_user>
    {
        public override ActionResult Create()
        {

            tbl_user item = new tbl_user();
            item.guid = Guid.NewGuid().ToString();

            return View(item);

        }

        public override ActionResult Create(tbl_user item)
        {
            // userType Change PostBack
            if (Request.Form["save"] == null)
            {
                return View(item);

            }

            // Facebook Ve Normal Üyeler
            if (item.userTypeId == 1 || item.userTypeId == 2)
            {

                if (string.IsNullOrWhiteSpace(item.Md5Converter) || item.Md5Converter.Length < 6)
                {
                    
                    ModelState.AddModelError("Md5Converter", "Şifre alanı en az 6 karakter olmalıdır.");
                    return View(item);
                }

                if (!item.gender.HasValue)
                {
                    ModelState.AddModelError("gender", "Cinsiyet girişi yapınızı.");
                    return View(item);
                }

                if (!item.birthday.HasValue)
                {
                    ModelState.AddModelError("birthday", "Doğum tarihi girişi yapınızı.");
                    return View(item);
                }

                item.password = MD5(item.Md5Converter);
            }

            item.createDate = DateTime.Now;

            return base.Create(item);
        }

        public override ActionResult Edit(int id)
        {
            return base.Edit(id);
        }

        public override ActionResult Edit(int id, tbl_user item)
        {
            // userType Change PostBack
            if (Request.Form["save"] == null)
            {
                return View(item);

            }

            var oldItem = getById(id);
            item.createDate = oldItem.createDate;

            // Facebook Ve Normal Üyeler
            if (item.userTypeId == 1 || item.userTypeId == 2)
            {

                if (!item.gender.HasValue)
                {
                    ModelState.AddModelError("gender", "Cinsiyet girişi yapınızı.");
                    return View(item);
                }

                if (!item.birthday.HasValue)
                {
                    ModelState.AddModelError("birthday", "Doğum tarihi girişi yapınızı.");
                    return View(item);
                }

                if (item.isPasswordUpdate)
                {
                    if (string.IsNullOrWhiteSpace(item.Md5Converter) || item.Md5Converter.Length < 6)
                    {
                        ModelState.AddModelError("Md5Converter", "Şifre alanı en az 6 karakter olmalıdır.");
                        return View(item);
                    }

                    item.password = MD5(item.Md5Converter);
                }
            }
            else
            {
                item.password = null;
                item.gender = null;
                item.birthday = null;
            }



            return base.Edit(id, item);
        }
    }
}
