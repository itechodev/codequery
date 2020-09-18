using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbTable<TTable>: IDbQueryable<TTable>
    {
        int Delete(Expression<Func<TTable, bool>> predicate);
        IDbUpdatable<TTable> Update(Expression<Func<TTable>> field, Expression<Func<TTable>> value);
        int Insert(IEnumerable<TTable> entries);
        int Insert(TTable entry);
    }
}