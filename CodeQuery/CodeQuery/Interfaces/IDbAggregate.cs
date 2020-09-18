using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CodeQuery.Interfaces
{
    public interface IDbAggregate<out TKey, TSource>
    {
        TAny Max<TAny>(Expression<Func<TSource, TAny>> field) where TAny : struct;
        TAny Min<TAny>(Expression<Func<TSource, TAny>> field) where TAny : struct;
        TAny Sum<TAny>(Expression<Func<TSource, TAny>> field) where TAny : struct;
        double Average<T, TAny>(Expression<Func<TSource, TAny>> field);
        
        TKey Key { get; }
    }

    public interface IDbAggregate2<out TKey, TSource, TSource2> 
    {
        TAny Max<TAny>(Expression<Func<TSource, TSource2, TAny>> field) where TAny : struct;
        TAny Min<TAny>(Expression<Func<TSource, TSource2, TAny>> field) where TAny : struct;
        TAny Sum<TAny>(Expression<Func<TSource, TSource2, TAny>> field) where TAny : struct;
        double Average<TAny>(Expression<Func<TSource, TSource2, TAny>> field);
        
        TKey Key { get; }
    }

    public interface IDbAggregate3<out TKey, TSource, TSource2, TSource3>
    {
        TAny Max<TAny>(Expression<Func<TSource, TSource2, TSource3, TAny>> field) where TAny : struct;
        TAny Min<TAny>(Expression<Func<TSource, TSource2, TSource3, TAny>> field) where TAny : struct;
        TAny Sum<TAny>(Expression<Func<TSource, TSource2, TSource3, TAny>> field) where TAny : struct;
        double Average<TAny>(Expression<Func<TSource, TSource2, TSource3, TAny>> field);
        
        TKey Key { get; }
    }

}