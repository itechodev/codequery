using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbJoinable3<TA, TB, TC>
    {
        IDbJoinable4<TA, TB, TC, T> Join<T>(IDbQueryable<T> join,  Expression<Func<TA, TB, TC, T, bool>> condition);
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TA, TB, TC, TSelect>> fields);
    }
}