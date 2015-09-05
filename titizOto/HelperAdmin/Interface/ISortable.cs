using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace HelperAdmin
{
    public interface ISortable<T>
    {
        void updateSequence(T item, int sequence);

        void updateSequence(List<T> item, int sequence);

        void moveUp(T item);

        void moveDown(T item);
    }
}