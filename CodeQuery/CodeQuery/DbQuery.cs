using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;
using CodeQuery.SqlExpressions;

namespace CodeQuery
{
    public class DbQuery<T> : SqlSource, IDbQueryable<T>
    {
        protected DbSchema Schema;
        public DbQuery(DbSchema schema, SqlSource parent, Type reflectedType, List<SqlColumnDefinition> columns) : base(parent, reflectedType, columns)
        {
            Schema = schema;
        }

        public IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<T, TSelect>> fields)
        {
            var pp = SqlExpressionParser.Parse(fields.Body, this);
            
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
}