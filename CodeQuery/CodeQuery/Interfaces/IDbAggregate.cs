using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public interface IDbAggregate<out TKey, TAggregate>
    {
        int Count(Expression<Func<TAggregate, object>> field);
        // Sum can be an int or double depending on the type
        TAny Sum<TAny>(Expression<Func<TAggregate, TAny>> field) where TAny: struct;
        double Average<TAny>(Expression<Func<TAggregate, TAny>> field) where TAny: struct;
        TKey Key { get; }
    }
}