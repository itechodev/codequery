using System;
using System.Linq.Expressions;
using codequery.Expressions;

namespace codequery.QuerySources
{
    public class QuerySource<T>
    {
        private SelectQuery _query { get; set; }

        public QuerySource()
        {
            _query = new SelectQuery();
        }

        public PostSelectQuerySource<N> Select<N>(Expression<Func<T, N>> fields)
        {
            //
            return new PostSelectQuerySource<N>();
        }

        public QuerySource<T> Where(Expression<Func<T, bool>> predicate)
        {
            // x => x.Field. > 10...
            return this;
        }
    }

    public class PostSelectQuerySource<T>
    {
        public T FetchSingle()
        {
            return default(T);
        }

        public T[] FetchArray()
        {
            return null;
        }

        // Selects, Where etc.
        // public QuerySource<T> From<T>(PostSelectQuerySource<T> source)
        // {
        //     return new QuerySource<T>();
        // }

    }
    
    public class Database
    {
        // Constant source queries
        public PostSelectQuerySource<T> Select<T>(T fields)
        {
            return new PostSelectQuerySource<T>();
        }

        // SubQuery
        public QuerySource<T> From<T>(PostSelectQuerySource<T> source)
        {
            return new QuerySource<T>();
        }

        // Tables to be declared in derived table
    }


    public class DatabaseTable<T> : QuerySource<T>
    {
        
    }
}