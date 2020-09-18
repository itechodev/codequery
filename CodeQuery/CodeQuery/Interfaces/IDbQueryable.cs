using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbQueryable<TSource>
    {
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TSource, TSelect>> fields);
        IDbQueryable2<TSource, TSource2> Join<TSource2>(JoinType joinType, IDbQueryable<TSource2> join, Expression<Func<TSource, TSource2, bool>> condition = null);
        IDbQueryable<IDbAggregate<TKey, TSource>> GroupBy<TKey>(Expression<Func<TSource, TKey>> order);
        IDbQueryable<TSource> Order(Expression<Func<TSource, object>> order, OrderBy orderBy);
        IDbQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate);
        IDbQueryFetchable<TSource> Union(IDbQueryable<TSource> other);
        IDbQueryFetchable<TSource> UnionAll(IDbQueryable<TSource> other);
        
        IDbQueryFetchable<TSource> SelectAll();
    }

    public interface IDbQueryable2<TSource, TSource2>
    {
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TSource, TSource2, TSelect>> fields);
        IDbQueryable3<TSource, TSource2, TSource3> Join<TSource3>(JoinType joinType, IDbQueryable<TSource3> join, Expression<Func<TSource, TSource2, TSource3, bool>> condition = null);
        IDbQueryable<IDbAggregate2<TKey, TSource, TSource2>> GroupBy<TKey>(Expression<Func<TSource, TSource2, TKey>> order);
        IDbQueryable2<TSource, TSource2> Order(Expression<Func<TSource, TSource2, object>> order, OrderBy orderBy);
        IDbQueryable2<TSource, TSource2> Where(Expression<Func<TSource, TSource2, bool>> predicate);
        IDbQueryFetchable<TSource> Union(IDbQueryable2<TSource, TSource2> other);
        IDbQueryFetchable<TSource> UnionAll(IDbQueryable2<TSource, TSource2> other);

    }
    
    public interface IDbQueryable3<TSource, TSource2, TSource3>
    {
        // IDbQueryable3<TSource, TSource2, TSource3,> Join<TSource3>(JoinType joinType, IDbQueryable<TSource3> join, Expression<Func<TSource, TSource2, TSource3, bool>> condition = null);
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TSource, TSource2, TSource3, TSelect>> fields);
        IDbQueryable<IDbAggregate3<TKey, TSource, TSource2, TSource3>> GroupBy<TKey>(Expression<Func<TSource, TSource2, TSource3, TKey>> order);
        IDbQueryable<TSource> Order(Expression<Func<TSource, TSource2, TSource3, object>> order, OrderBy orderBy);
        IDbQueryable<TSource> Where(Expression<Func<TSource, TSource2, TSource3, bool>> predicate);
        IDbQueryFetchable<TSource> Union(IDbQueryable3<TSource, TSource2, TSource3> other);
        IDbQueryFetchable<TSource> UnionAll(IDbQueryable3<TSource, TSource2, TSource3> other);
    }
}