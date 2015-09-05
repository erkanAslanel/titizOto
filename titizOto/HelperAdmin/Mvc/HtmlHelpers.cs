using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;


namespace System.Web.Mvc.Html
{
    public static class HtmlHelpers
    {

        public static MvcHtmlString CkeditorHtml<TModel, TValue>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TValue>> exp)
        {
            string name = ExpressionHelper.GetExpressionText(exp);
            var result = exp.Compile().DynamicInvoke(instance.ViewData.Model);

            string ckeditorTag;

            if (result != null)
            {
                ckeditorTag = "<textarea class=\"span12 ckeditor m-wrap\" name=\"[name]\" id=\"[name]\" rows=\"6\">" + result.ToString() + "</textarea>";
            }
            else
            {
                ckeditorTag = "<textarea class=\"span12 ckeditor m-wrap\" name=\"[name]\" id=\"[name]\" rows=\"6\">" + "</textarea>";
            }

            ckeditorTag = ckeditorTag.Replace("[name]", name);
            return new MvcHtmlString(ckeditorTag);
        }

        public static MvcHtmlString CkeditorHtml(this HtmlHelper instance, string name)
        {
            string ckeditorTag = "<textarea class=\"span12 ckeditor m-wrap\" name=\"[name]\" id=\"[name]\" rows=\"6\"></textarea>";
            ckeditorTag = ckeditorTag.Replace("[name]", name);
            return new MvcHtmlString(ckeditorTag);
        }

        public static MvcHtmlString CkeditorHtml(this HtmlHelper instance, string name, string value)
        {
            string ckeditorTag = "<textarea class=\"span12 ckeditor m-wrap\" name=\"[name]\" id=\"[name]\" rows=\"6\">" + value + "</textarea>";
            ckeditorTag = ckeditorTag.Replace("[name]", name);
            return new MvcHtmlString(ckeditorTag);
        }

        public static MvcHtmlString Uploader<TModel, TValue>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TValue>> exp, string path)
        {
            string name = ExpressionHelper.GetExpressionText(exp);
            string uploaderTag = "<div class=\"uploader\" data-param=\"[path]\" data-dom=\"#[name]\">You browser doesn't have HTML 4 support.</div>";
            uploaderTag = uploaderTag.Replace("[name]", name);
            uploaderTag = uploaderTag.Replace("[path]", path);
            uploaderTag += instance.HiddenFor(exp, new { id = name });
            return new MvcHtmlString(uploaderTag);
        }

        public static MvcHtmlString Uploader(this HtmlHelper instance, string name, string path)
        {

            string uploaderTag = "<div class=\"uploader\" data-param=\"[path]\" data-dom=\"#[name]\">You browser doesn't have HTML 4 support.</div>";
            uploaderTag = uploaderTag.Replace("[name]", name);
            uploaderTag = uploaderTag.Replace("[path]", path);
            uploaderTag += instance.Hidden(name, new { id = name });
            return new MvcHtmlString(uploaderTag);
        }

        public static MvcHtmlString Uploader(this HtmlHelper instance, string name, string path, string value)
        {
            string uploaderTag = "<div class=\"uploader\" data-param=\"[path]\" data-dom=\"#[name]\">You browser doesn't have HTML 4 support.</div>";
            uploaderTag = uploaderTag.Replace("[name]", name);
            uploaderTag = uploaderTag.Replace("[path]", path);
            uploaderTag += instance.Hidden(name, value, new { id = name });
            return new MvcHtmlString(uploaderTag);
        }


        public static MvcHtmlString UploaderWithThumbnailPreview<TModel, TValue>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TValue>> exp, string path,   Expression<Func<TModel, TValue>> expCoordinate, string width, string height, string mainPath)
        {
            string name = ExpressionHelper.GetExpressionText(exp);
            string coordinateName = ExpressionHelper.GetExpressionText(expCoordinate);  
            var val = exp.Compile().DynamicInvoke(instance.ViewData.Model);
            var valCoordianate = expCoordinate.Compile().DynamicInvoke(instance.ViewData.Model);

            string uploaderTag = "<div class=\"thumbnailImageContainer photoUploadContainer [addClass]\">  <h2>Ölçü Fotoğraf([widht]x[height])</h2> <div class=\"preview-pane\"> <div class=\"preview-container\"> <img src=\"[imageSource]\" /> </div> </div> </div>  <div style=\"clear: both; height: 1px;\">&nbsp;</div>  <div class=\"mainImageContainer photoUploadContainer [addClass]\"><a class=\"switchFull\">Tam Ekrana Geç</a><a class=\"switchBack\">Geri Dön</a>  <h2>Ana Fotoğraf</h2> <div>  <img src=\"[imageSource]\" class=\"target\" /> </div> </div>  <div style=\"clear: both; height: 1px;\">&nbsp;</div>  <a class=\"clearPhoto\">Fotoğrafı Sil</a> <div class=\"uploaderThumbnail [editMode]\" data-param=\"[path]\" data-dom=\"#[name]\">You browser doesn't have HTML 4 support.</div>";
            uploaderTag = uploaderTag.Replace("[name]", name);
            uploaderTag = uploaderTag.Replace("[widht]", width);
            uploaderTag = uploaderTag.Replace("[height]", height);
            uploaderTag = uploaderTag.Replace("[path]", path); 

            string photoName = "";
            if (val != null)
            {
                photoName = val.ToString();
                uploaderTag = uploaderTag.Replace("[addClass]", "showBlock");
                uploaderTag = uploaderTag.Replace("[editMode]", "editMode");

                
            }
            else
            {
                uploaderTag = uploaderTag.Replace("[addClass]", "noneBlock");
                uploaderTag = uploaderTag.Replace("[editMode]", "");
            }

            uploaderTag = uploaderTag.Replace("[imageSource]", mainPath + "Download/item/" + path + "/" + photoName);  
            uploaderTag += instance.HiddenFor(exp, new { id = name });
            uploaderTag += instance.Hidden(coordinateName, valCoordianate, new { @class = "coordinate", data_width = width.ToString(), data_height = height.ToString() }); 
            return new MvcHtmlString(uploaderTag);

        } 

        public static MvcHtmlString LangDropDown(this HtmlHelper instance)
        {
            return instance.DropDownList("langId", new SelectList(getLangData(),
              "Key", "Value",
              "1"), new { @class = "ddlLang" });
        }

        public static MvcHtmlString LangDropDown(this HtmlHelper instance, int selectValue)
        {
            return instance.DropDownList("langId", new SelectList(getLangData(),
                 "Key", "Value",
                 selectValue.ToString()), new { @class = "ddlLang" });
        }

        public static MvcHtmlString LangDropDown<TModel, TValue>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TValue>> exp)
        {
            var result = exp.Compile().DynamicInvoke(instance.ViewData.Model);

            if (result != null)
            {
                int selectedId;
                if (int.TryParse(result.ToString(), out selectedId))
                {
                    return LangDropDown(instance, selectedId);
                }
            }

            return LangDropDown(instance);

        }

        //public static MvcHtmlString StatuDropDown<TModel>(this HtmlHelper<TModel> instance, Expression<Func<TModel, bool>> exp)
        //{

        //    var result = exp.Compile().DynamicInvoke(instance.ViewData.Model);

        //    if (result != null)
        //    {

        //        if ((bool)result)
        //        {
        //            return new MvcHtmlString(instance.DropDownList("ddl" + exp, new SelectList(getStatuData(),
        //                       "Key", "Value", "1"), new { @class = "ddlStatu" }).ToString() + instance.HiddenFor(exp, new { @class = "statuCheck" }).ToString());
        //        }
        //        else
        //        {
        //            return new MvcHtmlString(instance.DropDownList("ddl" + exp, new SelectList(getStatuData(),
        //          "Key", "Value", "0"), new { @class = "ddlStatu" }).ToString() + instance.HiddenFor(exp, new { @class = "statuCheck" }).ToString());
        //        }
        //    }
        //    return new MvcHtmlString("");
        //}

        public static MvcHtmlString StatuDropDown<TModel>(this HtmlHelper<TModel> instance, Expression<Func<TModel, bool>> exp)
        {

            return instance.DropDownListFor(exp, new SelectList(getStatuData(), "Key", "Value"));
        }

        public static MvcHtmlString StatuDropDown<TModel>(this HtmlHelper<TModel> instance, Expression<Func<TModel, bool?>> exp)
        {
            return instance.DropDownListFor(exp, new SelectList(getStatuData(), "Key", "Value"));
        }


        private static Dictionary<string, string> getLangData()
        {
            Dictionary<string, string> val = new Dictionary<string, string>();

            val.Add("1", "Türkçe");
           

            return val;
        }

        //private static Dictionary<string, string> getStatuData()
        //{
        //    Dictionary<string, string> val = new Dictionary<string, string>();

        //    val.Add("0", "Pasif");
        //    val.Add("1", "Aktif");

        //    return val;
        //}

        private static Dictionary<string, string> getStatuData()
        {
            Dictionary<string, string> val = new Dictionary<string, string>();

            val.Add("False", "Pasif");
            val.Add("True", "Aktif");

            return val;
        }

        public static MvcHtmlString getStatus<TModel, TValue>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TValue>> exp)
        {
            var result = exp.Compile().DynamicInvoke(instance.ViewData.Model);
            if (result != null)
            {
                return getStatus((bool)result);
            }

            return new MvcHtmlString("");
        }

        public static MvcHtmlString getStatus(bool statu)
        {
            if (!statu)
            {
                return new MvcHtmlString("<span class=\"bullet bullet-red\"></span>");
            }
            else
            {
                return new MvcHtmlString("<span class=\"bullet bullet-green\"></span>");
            }


        }

        public static MvcHtmlString getStatus(this HtmlHelper html, bool statu)
        {
            return getStatus(statu);


        }

        public static string getLangName(this HtmlHelper html, int langId)
        {
            return getLangName(langId);
        }

        public static string getLangName(int langId)
        {
            if (langId == 1)
            {
                return "Türkçe";
            }
            if (langId == 2)
            {
                return "İngilizce";
            }

            return "";
        }

        public static string getLangName<TModel, TValue>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TValue>> exp)
        {
            var result = exp.Compile().DynamicInvoke(instance.ViewData.Model);

            if (result != null)
            {
                return getLangName((int)result);
            }

            return "";
        }

        public static MvcHtmlString Datepicker<TModel, TValue>(this HtmlHelper<TModel> instance, Expression<Func<TModel, TValue>> exp)
        {
            string name = ExpressionHelper.GetExpressionText(exp);
            var result = exp.Compile().DynamicInvoke(instance.ViewData.Model);

            if (result == null)
            {
                return instance.TextBox(name, null, new { @class = "datepicker" });
            }
            else
            {
                return instance.TextBox(name, ((DateTime)result).ToShortDateString(), new { @class = "datepicker" });
            }

        }
    }
}