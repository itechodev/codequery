using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeQuery.Interfaces;

namespace CodeQuery
{
    public class DbTable<T> : IDbTable<T>
    {
        private SqlQuerySelect _query;

        public DbTable(SqlQuerySelect query = null)
        {
            _query = query ?? new SqlQuerySelect();
        }

        public IDbJoinable2<T, T1> Join<T1>(JoinType joinType, IDbQueryable<T1> @join, Expression<Func<T, T1, bool>> condition = null)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable<IDbAggregate<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> order)
        {
            // add group by in SqlQuerySelect
            _query.GroupBy = "GroupBy";
            return new DbTable<IDbAggregate<TKey, T>>(_query);
        }

        public IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<T, TSelect>> fields)
        {
            // .Select(e => new
            // {
            //     UserId = e.Key,
            //     Used = e.Count(null)
            // })
            
            // Check for aggregates
            // add to list

            // return this;
            return null;
        }

        public IDbQueryFetchable<T> SelectAll()
        {
            throw new NotImplementedException();
        }

        public IDbQueryable<T> Order(Expression<Func<T, T>> order, OrderBy orderBy)
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