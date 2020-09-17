using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbAggregate<TSource>
    {
        int Count(Expression<Func<TSource, TSource>> field);
        double Sum();
        double Average();
        TSource Key { get; }
    }
}