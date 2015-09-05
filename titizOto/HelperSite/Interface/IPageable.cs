using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelperSite.Interface
{
    public interface IPageable
    {
        void setTitle(string text);
        void setBrowserTitle(string text);
        void setDescription(string text); 
        void setMeta(string text);
        void setKeywords(string text);
        void setPageId(int pageId);
         
        string getBrowserTitle();
        string getDescription();
        string getMeta();
        string getKeywords();
        int getPageId();
    }
}