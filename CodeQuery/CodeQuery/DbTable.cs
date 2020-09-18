using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeQuery.Interfaces;

namespace CodeQuery
{
    public class DbTable<T> : IDbTable<T>, IDbSource
    {
        private SqlQuerySelect _query;

        public DbTable(SqlQuerySelect query = null)
        {
            _query = query ?? new SqlQuerySelect();
        }


        public IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<T, TSelect>> fields)
        {
            throw new NotImplementedException();
        }

        public IDbQueryable2<T, TSource2> Join<TSource2>(JoinType joinType, IDbQueryable<TSource2> @join, Expression<Func<T, TSource2, bool>> condition = null)
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