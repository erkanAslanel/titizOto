using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HelperAdmin
{
    public class TemplateHtmlGenerator
    { 
        private Dictionary<string, string> htmlList { get; set; }

        public List<string> getHtmlList(Type typeInfo)
        {
            List<string> htmlList = new List<string>();


            foreach (PropertyInfo propertyInfo in typeInfo.GetProperties())
            {
                foreach (var item in propertyInfo.GetCustomAttributes(typeof(DataTypeAttribute), false))
                {
                    string dataTypeName = ((DataTypeAttribute)item).CustomDataType;

                    htmlList.Add(getTypeHtmlList(dataTypeName, propertyInfo.Name));

                }
            }



            return htmlList;
        }

        private string getTypeHtmlList(string key, string replaceFieldName)
        {
            if (htmlList == null || htmlList.Count == 0)
            {
                Dictionary<string, string> list = new Dictionary<string, string>();

                string itemKey = "urlName";
                string itemVal = "<!--[fieldName] -->[lineBreak]<div class=\"rowElem\"> @Html.LabelFor(a => a.[fieldName]) <div class=\"formRight\">@Html.TextBoxFor(a => a.[fieldName], new { @class = \"changeUrl\" })<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "urlNameEn";
                itemVal = "<!--[fieldName] -->[lineBreak]<div class=\"rowElem\"> @Html.LabelFor(a => a.[fieldName]) <div class=\"formRight\">@Html.TextBoxFor(a => a.[fieldName], new { @class = \"changeUrlEn\" })<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);


                itemKey = "statu";
                itemVal = "<!--[fieldName] -->[lineBreak] <div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.StatuDropDown(a => a.[fieldName])<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "htmlContent";
                itemVal = "<!--[fieldName] -->[lineBreak] <div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.CkeditorHtml(a => a.[fieldName])<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);


                itemKey = "primaryKey";
                itemVal = "<!--[fieldName] -->[lineBreak]   @Html.HiddenFor(a => a.[fieldName], new { @id = \"primaryKey\" })";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "url";
                itemVal = "<!--[fieldName] -->[lineBreak] <div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.TextBoxFor(a => a.[fieldName], new { @class = \"pageUrl\", @readonly = true, @data_action = Url.Action(\"UrlControl\") }) <span class=\"mt5 ml10 inlineBlock\">Sayfa Adresini Kendim Oluşturmak İstiyorum</span> @Html.CheckBox(\"isManuelUrl\", Model.isManuelUrl, new { @class = \"manuelUrl\" })<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "urlEn";
                itemVal = "<!--[fieldName] -->[lineBreak] <div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.TextBoxFor(a => a.[fieldName], new { @class = \"pageUrlEn\", @readonly = true, @data_action = Url.Action(\"UrlControlEn\") }) <span class=\"mt5 ml10 inlineBlock\">Sayfa Adresini Kendim Oluşturmak İstiyorum</span> @Html.CheckBox(\"enIsManuelUrl\", Model.enIsManuelUrl, new { @class = \"isManuelUrlEn\" })<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "uploader";
                itemVal = "<!--[fieldName] -->[lineBreak]  @Html.Uploader(a => a.[fieldName], \"[replaceClassName]\")<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label><div class=\"fix\"></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "seoDescription";
                itemVal = "<!--[fieldName] -->[lineBreak]<div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.TextAreaFor(a => a.[fieldName])<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);


                itemKey = "seoKeyword";
                itemVal = "<!--[fieldName] -->[lineBreak] <div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.TextBoxFor(a => a.[fieldName], new { @class = \"tags\" })</div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);


                itemKey = "selectBox";
                itemVal = "<!--[fieldName] -->[lineBreak]  <div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.ListBox(\"helper[fieldName]\", Model.[fieldName]SelectItems)<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "lang";
                itemVal = "<!--[fieldName] -->[lineBreak]<div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.LangDropDown(a => a.[fieldName])<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "date";
                itemVal = "<!--[fieldName] -->[lineBreak]<div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.Datepicker(a => a.[fieldName])<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName]) </label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "dropDown";
                itemVal = "<!--[fieldName] -->[lineBreak] <div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.DropDownList(\"[fieldName]\", new SelectList(Model.[fieldName]List(), \"key\", \"value\"))</div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "normalText";
                itemVal = "<!--[fieldName] -->[lineBreak]  <div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.TextBoxFor(a => a.[fieldName])<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);


                itemKey = "categoryList";
                itemVal = "<!--[fieldName] -->[lineBreak] <div class=\"categoryContainer\">@if (ViewBag.treeView != null){TreeviewOption option = (TreeviewOption)ViewBag.treeView; <div id=\"tree\"></div><input id=\"@option.fieldName\" name=\"@option.fieldName\" type=\"hidden\" /> <label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label>@section scriptArea{@Html.Partial(\"treeviewHtml\", option)}}</div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                itemKey = "bigText";
                itemVal = "<!--[fieldName] -->[lineBreak]<div class=\"rowElem\">@Html.LabelFor(a => a.[fieldName])<div class=\"formRight\">@Html.TextAreaFor(a => a.[fieldName],new { style=\"height:100px\" })<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label></div><div class=\"fix\"></div></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                 itemKey = "photoCut";
                 itemVal = "<!--[fieldName] -->[lineBreak] @Html.UploaderWithThumbnailPreview(a => a.[fieldName], [fieldNameCoordinate], a => a.[fieldNameReplace], \"px\",  \"px\", Url.Content(\"~/\"))<label class=\"error\">@Html.ValidationMessageFor(a => a.[fieldName])</label><div class=\"fix\"></div>";
                itemVal = itemVal.Replace("[lineBreak]", Environment.NewLine);
                list.Add(itemKey, itemVal);

                 


                htmlList = list;
            }

            return htmlList[key].Replace("[fieldName]", replaceFieldName);


        }  
    }
}