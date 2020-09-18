using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbQueryable<TSource>
    {
        // Joins
        IDbJoinable2<TSource, T> Join<T>(JoinType joinType, IDbQueryable<T> join,  Expression<Func<TSource, T, bool>> condition = null);
        // Aggregation
        IDbQueryable<IDbAggregate<TKey, TSource>> GroupBy<TKey>(Expression<Func<TSource, TKey>> order);
        // Select fields
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TSource, TSelect>> fields);
        IDbQueryFetchable<TSource> SelectAll();
        // Order
        IDbQueryable<TSource> Order(Expression<Func<TSource, TSource>> order, OrderBy orderBy);
        // Where
        IDbQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate);
        
        // Unions
        IDbQueryFetchable<TSource> Union(IDbQueryable<TSource> other);
        IDbQueryFetchable<TSource> UnionAll(IDbQueryable<TSource> other);
    }
}