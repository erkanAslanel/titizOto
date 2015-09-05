using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperAdmin;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)] 
    public class UploadController : Controller
    {
        [HttpPost, ValidateAntiForgeryToken]
        public string Upload(string param)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                string fileName = System.IO.Path.GetFileName(Request.Files[0].FileName);
                string extention = System.IO.Path.GetExtension(Request.Files[0].FileName).ToLower();

                if (extention == ".jpg" || extention == ".png" || extention == ".pdf")
                {
                    fileName = generateRandomFileName(fileName);

                    Request.Files[0].SaveAs(Server.MapPath("~/Download/item/" + param + "/" + fileName));

                    return fileName;
                }
                else
                {
                    return "";
                }

            }

            return "";
        }

        public string generateRandomFileName(string fileName)
        {
            fileName = fileName.ToLower();
            fileName = fileName.Replace(" ", null);
            fileName = fileName.Replace("ı", "i");
            fileName = fileName.Replace("ü", "u");
            fileName = fileName.Replace("ğ", "g");
            fileName = fileName.Replace("ç", "c");
            fileName = fileName.Replace("ö", "o");
            fileName = fileName.Replace("ş", "s");



            fileName = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString().Substring(0, 5) + fileName;

            return fileName;
        } 
    }
}
