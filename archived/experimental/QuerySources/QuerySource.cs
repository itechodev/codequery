using System;
using System.Linq.Expressions;

namespace codequery.QuerySources
{
    public enum WhereType
    {
        Or,
        And
    }

    public class QuerySource<T>
    {
        public ResultQuerySource<T> SelectAll()
        {
            return null;
        }

        public ResultQuerySource<N> Select<N>(Expression<Func<T, N>> fields)
        {
            return null;
        }

        public AggregateSource<T, N> GroupBy<N>(Expression<Func<T, N>> groupBy)
        {
            return null;
        }

        public QuerySource<T> Where(Expression<Func<T, bool>> predicate, WhereType type = WhereType.And)
        {
            // x => x.Field. > 10...
            // .Where(s => s.Active)
            // .Where(s => s.UID.Contains("11"))
            return null;
        }

        public QuerySource<T, A> InnerJoin<A>(Func<A> select)
        {
            return null;
        }

        public QuerySource<T, A> LeftJoin<A>(Func<A> select) where A: class
        {
            return null;
        }

        public QuerySource<T, Nullable<A>> LeftJoin<A>(Func<Nullable<A>> select) where A: struct
        {
            return null;
        }

        public QuerySource<T, A> RightJoin<A>(Func<A> select) where A: class
        {
            return null;
        }
        
        public QuerySource<T, Nullable<A>> RightJoin<A>(Func<Nullable<A>> select) where A: struct
        {
            return null;
        }
    }

    public class QuerySource<A, B>
    {
        public QuerySource<A, B, C> InnerJoin<C>(Func<C> select)
        {
            return null;
        }
    }
    public class QuerySource<A, B, C>
    {
        public QuerySource<A, B, C, D> InnerJoin<D>(Func<D> select)
        {
            return null;
        }
    }

     public class QuerySource<A, B, C, D>
    {
        public G Select<G>(Func<A, B, C, D, G> exp)
        {
            return default(G);
        }
    }
}