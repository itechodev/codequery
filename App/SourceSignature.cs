using System;
using System.Linq.Expressions;

namespace codequery.App
{
    public interface IQuerySource<T>
    {
        IResultSource<T> SelectAll();
        IResultSource<F> Select<F>(Expression<Func<T, F>> fields);
        IQuerySource<T> Where(Expression<Func<T, bool>> predicate);
        IQuerySource<T> Order(Expression<Action<T>> predicate);
        IQuerySource<IAggregate<Key, T>> GroupBy<Key>(Expression<Func<T, Key>> groupBy);
        IJoinSource<T, R> Join<R>(IQuerySource<R> right);
    }

    public interface IJoinSource<A, B>
    {
        IResultSource<(A, B)> SelectAll();
        IResultSource<F> Select<F>(Expression<Func<A, B, F>> fields);
        IJoinSource<A, B> Where(Expression<Func<A, B, bool>> predicate);
        IJoinSource<A, B> Order(Expression<Action<A, B>> predicate);
        IQuerySource<IAggregate<Key, (A, B)>> GroupBy<Key, Result>(Expression<Func<A, B, Key>> groupBy);
    }

    public interface IResultSource<T>
    {
        IResultSource<T> Union(T fields);
        IResultSource<T> UnionAll(T fields);
        T FetchSingle();
        T[] FetchArray();
    }

    public interface IAggregate<Key, Agg>
    {
        Key Value { get; set; }
        int Count();
        int CountDistinct();
        int? Average(Expression<Func<Agg, int>> field);
        double? Average(Expression<Func<Agg, double>> field);
        int? Sum(Expression<Func<Key, int>> field);
        double? Sum(Expression<Func<Key, double>> field);
        P Max<P>(Expression<Func<Agg, P>> clause);
        P Min<P>(Expression<Func<Agg, P>> clause);
    }
}