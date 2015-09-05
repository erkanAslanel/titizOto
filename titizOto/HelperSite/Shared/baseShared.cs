using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelperSite.Shared
{
    public abstract class baseShared
    {
        protected titizOto.Models.titizOtoEntities db;

        public Tuple<bool, string> getValidationResult(List<ValidationResult> list)
        {
            return new Tuple<bool, string>((list.Count == 0), string.Join("<br />", list.Select(a => a.ErrorMessage).ToList()));
        }
    }
}