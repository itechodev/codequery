using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbUpdatable<TTable>
    {
        IDbUpdatable<TTable> Update(Expression<Func<TTable>> field, Expression<Func<TTable>> value);
        IDbUpdatable<TTable> Where(Expression<Func<TTable, bool>> predicate);
        int Update();
    }
}