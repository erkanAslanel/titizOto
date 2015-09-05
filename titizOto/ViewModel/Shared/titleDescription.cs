using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Shared
{
    public class titleDescription : HelperSite.Interface.IPageable
    {
        public string title { get; set; }
        public string description { get; set; }
        public int pageId { get; set; }
        public string meta { get; set; }
        public string keyword { get; set; }
        public string browserTitle { get; set; }

        public void setTitle(string text)
        {
            this.title = text;
        }

        public void setDescription(string text)
        {

            this.description = text;
        }

        public void setMeta(string text)
        {
            this.meta = text;
        }

        public void setKeywords(string text)
        {
            this.keyword = text;
        }

        public void setPageId(int pageId)
        {
            this.pageId = pageId;
        }

        public void setBrowserTitle(string text)
        {
            this.browserTitle = text;
        } 

        public string getBrowserTitle()
        {
            return browserTitle;
        }

        public string getDescription()
        {
            return description;
        }

        public string getMeta()
        {
            return meta;
        }

        public string getKeywords()
        {
            return keyword;
        }

        public int getPageId()
        {
            return pageId;
        }
    }
}