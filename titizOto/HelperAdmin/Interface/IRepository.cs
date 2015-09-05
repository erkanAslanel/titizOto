using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace HelperAdmin
{
    public interface IRepository<T>
    {
        void add(T item);

        void editItem(int id,T item);

        void deleteItem(T item);

        void changeStatu(T item, bool changeToStatu);

        T getBy(Expression<Func<T, bool>> filter);

        List<T> getList();
    }


}