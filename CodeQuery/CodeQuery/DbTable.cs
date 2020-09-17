using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeQuery
{
    public class DbTable<T> : IDbTable<T>
    {
        private SqlQuerySelect _query;

        public DbTable(SqlQuerySelect query = null)
        {
            _query = new SqlQuerySelect() ?? query;
        }

        public IDbJoinable2<T, T1> Join<T1>(JoinType joinType, IDbQueryable<T1> @join, Expression<Func<T, T1, bool>> condition = null)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable<IDbAggregate<T1>> GroupBy<T1>(Expression<Func<T, T1>> order)
        {
            // add group by in SqlQuerySelect
            _query.GroupBy = "GroupBy";
            return new DbTable<IDbAggregate<T1>>(_query);
        }

        public IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<T, TSelect>> fields)
        {
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