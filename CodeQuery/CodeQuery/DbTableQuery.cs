using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;

namespace CodeQuery
{
    // The starting point of a query
    // It is both a table - support CRUD statements and a DbQuery
    public class DbTableQuery<T> : DbQuery<T>, IDbTable<T> 
    {
        private SqlQuerySelect _query;

        public DbTableQuery(DbSchema schema, SqlTableDefinition def) : base(schema, null, def.ReflectedType, def.Columns)
        {
            _query = new SqlQuerySelect(this);
        }

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IDbUpdatable<T> Update(Expression<Func<T, object>> field, Expression<Func<T, object>> value)
        {
            throw new NotImplementedException();
        }

        void IDbTable<T>.Insert(IEnumerable<T> entries)
        {
            throw new NotImplementedException();
        }

        void IDbTable<T>.Insert(T entry)
        {
            throw new NotImplementedException();
        }

        public void Insert(IDbQueryable<T> query)
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