using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeQuery.Interfaces;

namespace CodeQuery
{
    public class DbQuery<T> : IDbTable<T>
    {
        private SqlQuerySelect _query;

        public DbQuery(DbSchema schema, SqlQuerySelect query)
        {
            _query = query;
        }
        
        public IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<T, TSelect>> fields)
        {
            // select expressions from sources
            // t => new {t.Added, t.Count}
            ExpressionPrinter.Print(fields);

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

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IDbUpdatable<T> Update(Expression<Func<T>> field, Expression<Func<T>> value)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<T> entries)
        {
            throw new NotImplementedException();
        }

        public int Insert(T entry)
        {
            throw new NotImplementedException();
        }
    }
}