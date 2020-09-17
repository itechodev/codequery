using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbJoinable4<TA, TB, TC, TD>
    {
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TA, TB, TC, TD, TSelect>> fields);
    }
}