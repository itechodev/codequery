using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbQueryable<TSource>
    {
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TSource, TSelect>> fields);

        IDbQueryable2<TSource, TSource2> Join<TSource2>(JoinType joinType, IDbQueryable<TSource2> join, Expression<Func<TSource, TSource2, bool>> condition = null);

        // IDbQueryable<IDbAggregate<TKey, TSource>> GroupBy<TKey>(Expression<Func<TSource, TKey>> order);

        // IDbAggregate<TKey, TSource> GroupBy<TKey>(Expression<Func<TSource, TKey>> order);
        //
        // IDbQueryable<TSource> Order(Expression<Func<TSource, TSource>> order, OrderBy orderBy);
        // IDbQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate);
        // IDbQueryFetchable<TSource> Union(IDbQueryable<TSource> other);
        // IDbQueryFetchable<TSource> UnionAll(IDbQueryable<TSource> other);
        //
        // IDbQueryFetchable<TSource> SelectAll();
    }

    public interface IDbQueryable2<TSource, TSource2>
    {
        // IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TSource, TSource2, TSelect>> fields);
        
        // IDbJoinable3<TSource, TSource2, TSource3> Join<TSource3>(JoinType joinType, IDbQueryable<TSource3> join,  Expression<Func<TSource, TSource2, bool>> condition = null);
        // IDbQueryable<IDbAggregate<TKey, TSource>> GroupBy<TKey>(Expression<Func<TSource, TKey>> order);
        // IDbQueryable<TSource> Order(Expression<Func<TSource, TSource>> order, OrderBy orderBy);
        // IDbQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate);
        // IDbQueryFetchable<TSource> Union(IDbQueryable<TSource> other);
        // IDbQueryFetchable<TSource> UnionAll(IDbQueryable<TSource> other);
    }
    
    public interface IDbQueryable3<TSource, TSource2, TSource3>
    {
        // IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TSource, TSource2, TSource3, TSelect>> fields);
    }
}