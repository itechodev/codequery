using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;
using CodeQuery.SqlExpressions;

namespace CodeQuery
{
    public class DbQuery<T> : IDbQueryable<T>
    {
        private SqlQuerySelect _query;

        public DbQuery(SqlSource source)
        {
            _query = new SqlQuerySelect()
            {
                Source = source
            };
        }

        public SqlSource SqlSource()
        {
            return _query.Source;
        }

        public IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<T, TSelect>> fields)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable2<T, TSource2> InnerJoin<TSource2>(IDbQueryable<TSource2> @join, Expression<Func<T, TSource2, bool>> condition = null)
        {
            var source = new SqlJoinSource()
            {
                Left = _query.Source,
                Right = @join.SqlSource() 
                // Condition
                // Type
            };
            return new DbQuery2<T, TSource2>(source);
        }

        public IDbQueryable2<T, TSource2> LeftJoin<TSource2>(IDbQueryable<TSource2> @join, Expression<Func<T, TSource2, bool>> condition = null)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable2<T, TSource2> RightJoin<TSource2>(IDbQueryable<TSource2> @join, Expression<Func<T, TSource2, bool>> condition = null)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable2<T, TSource2> CrossJoin<TSource2>(IDbQueryable<TSource2> @join, Expression<Func<T, TSource2, bool>> condition = null)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable2<T, TSource2> Join<TSource2>(JoinType joinType, IDbQueryable<TSource2> @join, Expression<Func<T, TSource2, bool>> condition = null)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable2<T, TSource2> Join<TSource2>(JoinType joinType, Expression<Func<T, TSource2, bool>> condition = null) where TSource2 : IDbSource
        {
            throw new NotImplementedException();
        }

        public IDbQueryable<IDbAggregate<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> order)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable<T> Order(Expression<Func<T, object>> order, OrderBy orderBy)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IDbQueryFetchable<T> Union(IDbQueryable<T> other)
        {
            throw new NotImplementedException();
        }

        public IDbQueryFetchable<T> UnionAll(IDbQueryable<T> other)
        {
            throw new NotImplementedException();
        }

        public IDbQueryFetchable<T> SelectAll()
        {
            throw new NotImplementedException();
        }
    }

    public class DbQuery2<T1, T2> : IDbQueryable2<T1, T2>
    {
        private readonly SqlQuerySelect _query;

        public DbQuery2(SqlSource source)
        {
            _query = new SqlQuerySelect()
            {
                Source = source
            };
        }

        public IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<T1, T2, TSelect>> fields)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable3<T1, T2, TSource3> Join<TSource3>(JoinType joinType, IDbQueryable<TSource3> @join, Expression<Func<T1, T2, TSource3, bool>> condition = null)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable<IDbAggregate2<TKey, T1, T2>> GroupBy<TKey>(Expression<Func<T1, T2, TKey>> order)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable2<T1, T2> Order(Expression<Func<T1, T2, object>> order, OrderBy orderBy)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable2<T1, T2> Where(Expression<Func<T1, T2, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IDbQueryFetchable<T1> Union(IDbQueryable2<T1, T2> other)
        {
            throw new NotImplementedException();
        }

        public IDbQueryFetchable<T1> UnionAll(IDbQueryable2<T1, T2> other)
        {
            throw new NotImplementedException();
        }
    }
}