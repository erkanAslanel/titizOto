using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace HelperAdmin
{
    public abstract class DbConnector : IDisposable
    {
        internal titizOtoEntities db;

        public DbConnector()
        {
            db = new titizOtoEntities();
            db.Configuration.LazyLoadingEnabled = true;
        }

        void IDisposable.Dispose()
        {
            db.Dispose();
        }

    }
}