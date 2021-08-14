using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbTable<TTable>: IDbQueryable<TTable>
    {
        int Delete(Expression<Func<TTable, bool>> predicate);
        IDbUpdatable<TTable> Update(Expression<Func<TTable, object>> field, Expression<Func<TTable, object>> value);
        void Insert(IEnumerable<TTable> entries);
        void Insert(TTable entry);
        void Insert(IDbQueryable<TTable> query);
    }
}