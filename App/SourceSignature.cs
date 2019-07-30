using System;
using System.Linq.Expressions;

namespace codequery.App
{
    public interface IQuerySource<A>
    {
        IResultSource<A> SelectAll();
        IResultSource<F> Select<F>(Expression<Func<A, F>> fields);
        IQuerySource<A> Where(Expression<Func<A, bool>> predicate);
        IQuerySource<A> Order(Expression<Action<A>> predicate);
        IQuerySource<IAggregate<Key, A>> GroupBy<Key>(Expression<Func<A, Key>> groupBy);
        IJoinSource<A, R> Join<R>(IQuerySource<R> right);
    }

    public interface IJoinSource<A, B>
    {
        IResultSource<(A, B)> SelectAll();
        IResultSource<F> Select<F>(Expression<Func<A, B, F>> fields);
        IJoinSource<A, B> Where(Expression<Func<A, B, bool>> predicate);
        IJoinSource<A, B> Order(Expression<Action<A, B>> predicate);
        IQuerySource<IAggregate<Key, A, B>> GroupBy<Key>(Expression<Func<A, B, Key>> key);
    }

    public interface IResultSource<T>
    {
        IResultSource<T> Union(IResultSource<T> fields);
        IResultSource<T> UnionAll(IResultSource<T> fields);
        T FetchSingle();
        T[] FetchArray();
    }

    public interface IAggregate<Key, A>
    {
        Key Value { get; set; }
        int Count();
        int CountDistinct();
        int? Average(Expression<Func<A, int>> field);
        double? Average(Expression<Func<A, double>> field);
        int? Sum(Expression<Func<A, int>> field);
        double? Sum(Expression<Func<A, double>> field);
        P Max<P>(Expression<Func<A, P>> clause);
        P Min<P>(Expression<Func<A, P>> clause);
    }

    public interface IAggregate<Key, A, B>
    {
        Key Value { get; set; }
        int Count();
        int CountDistinct();
        int? Average(Expression<Func<A, B, int>> field);
        double? Average(Expression<Func<A, B, double>> field);
        int? Sum(Expression<Func<A, B, int>> field);
        double? Sum(Expression<Func<A, B, double>> field);
        P Max<P>(Expression<Func<A, B, P>> clause);
        P Min<P>(Expression<Func<A, B, P>> clause);
    }
}