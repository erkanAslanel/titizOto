using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace HelperSite.DbController
{
    public abstract class DbConnector : IDisposable
    {
        internal titizOtoEntities db;

        public DbConnector()
        {
            db = new titizOtoEntities();
            db.Configuration.LazyLoadingEnabled = false;
        }

        void IDisposable.Dispose()
        {
            db.Dispose();
        }

    }
}