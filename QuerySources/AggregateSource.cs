using System;
using System.Linq.Expressions;

namespace codequery.QuerySources
{
    public class AggregateSource<T, N>
    {
        public T Key { get; set; }
        public int Count()
        {
            return 0;
        }
        public int CountDistinct()
        {
            return 0;
        }
        public int? Average(Expression<Func<N, int>> field)
        {
            return null;
        }
        public double? Average(Expression<Func<N, double>> field)
        {
            return null;
        }
        public int? AverageDistinct(Expression<Func<N, int>> field)
        {
            return null;
        }
        public double? AverageDistinct(Expression<Func<N, double>> field)
        {
            return null;
        }   
        public int? Sum(Expression<Func<N, int>> field)
        {
            return null;
        }
        public double? Sum(Expression<Func<N, double>> field)
        {
            return null;
        }
        public P Max<P>(Expression<Func<N, P>> clause)
        {
            return default(P);
        }
        public P Min<P>(Expression<Func<N, P>> clause)
        {
            return default(P);
        }

    }
}