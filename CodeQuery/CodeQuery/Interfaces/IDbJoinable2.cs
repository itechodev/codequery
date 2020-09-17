using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbJoinable2<TA, TB>
    {
        // Join 
        IDbJoinable3<TA, TB, T> Join<T>(IDbQueryable<T> join,  Expression<Func<TA, TB, T, bool>> condition);
        
        // // Aggregation
        // IDbQueryable<IDbAggregate<(TA, TB)>> GroupBy<TKey>(Expression<Func<TA, TB, TKey>> order);
        // // Select fields
        // IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TA, TB, TSelect>> fields);
        // // Order
        // IDbJoinable2<TA, TB> Order(Expression<Func<TA, TB>> order, OrderBy orderBy);
        // // Where
        // IDbJoinable2<TA, TB> Where(Expression<Func<TA, TB, bool>> predicate);
    }
}