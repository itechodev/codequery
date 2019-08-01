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
        IAggregateSource<IAggregate<Key, A>> GroupBy<Key>(Expression<Func<A, Key>> groupBy);
        IJoinSource<A, R> Join<R>(IQuerySource<R> right);
    }

    public interface IUpdateTable<T>
    {
        IUpdateTable<T> Set<F>(Expression<Action<T>> field, Expression<Func<T, F>> value);
        IUpdateTable<T> Where(Expression<Func<T, bool>> predicate);
        bool Update();
    }

    public interface ITableSource<T>
    {
        IResultSource<T> SelectAll();
        IResultSource<F> Select<F>(Expression<Func<T, F>> fields);
        IJoinSource<T, R> Join<R>(IQuerySource<R> right);

        IUpdateTable<T> Set<F>(Expression<Action<T>> field, Expression<Func<T, F>> value);
        T Insert(T data);
        int InsertInto(IQuerySource<T> source);
        bool Delete<F>(Expression<Func<T, F>> clause);
    }

    public interface IJoinSource<A, B>
    {
        IResultSource<F> Select<F>(Expression<Func<A, B, F>> fields);
        IJoinSource<A, B> Where(Expression<Func<A, B, bool>> predicate);
        IJoinSource<A, B> Order(Expression<Action<A, B>> predicate);
        IAggregateSource<IAggregate<Key, A, B>> GroupBy<Key>(Expression<Func<A, B, Key>> key);
        IJoinSource<A, B, R> Join<R>(IQuerySource<R> right);
    }

    public interface IJoinSource<A, B, C>
    {
        IResultSource<F> Select<F>(Expression<Func<A, B, C, F>> fields);
        IJoinSource<A, B, C> Where(Expression<Func<A, B, C, bool>> predicate);
        IJoinSource<A, B, C> Order(Expression<Action<A, B, C>> predicate);
        IAggregateSource<IAggregate<Key, A, B, C>> GroupBy<Key>(Expression<Func<A, B, C, Key>> key);
        // IJoinSource<A, B, R> Join<R>(IQuerySource<R> right);
    }

    public interface IAggregateSource<T> : IQuerySource<T>
    {
        IQuerySource<T> Having(Expression<Func<T, bool>> predicate);
    }

    public interface IResultSource<T> : IQuerySource<T>
    {
        IResultSource<T> Union(IResultSource<T> fields);
        IResultSource<T> UnionAll(IResultSource<T> fields);
        T FetchSingle();
        T[] FetchArray(int skip = 0, int? take = null);
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

    public interface IAggregate<Key, A, B, C>
    {
        Key Value { get; set; }
        int Count();
        int CountDistinct();
        int? Average(Expression<Func<A, B, C, int>> field);
        double? Average(Expression<Func<A, B, C, double>> field);
        int? Sum(Expression<Func<A, B, C, int>> field);
        double? Sum(Expression<Func<A, B, C, double>> field);
        P Max<P>(Expression<Func<A, B, C, P>> clause);
        P Min<P>(Expression<Func<A, B, C, P>> clause);
    }
}