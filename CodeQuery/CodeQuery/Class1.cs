using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace CodeQuery
{
    public enum OrderBy
    {
        Ascending,
        Descending
    }

    public enum JoinType
    {
        Left,
        Right,
        Inner,
        FullOuter,
        Cross
    }

    public interface IDbAggregate<TSource>
    {
        int Count(Expression<Func<TSource, TSource>> field);
        double Sum();
        double Average();
        TSource Key { get; }
    }

    public interface IDbJoinable4<TA, TB, TC, TD>
    {
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TA, TB, TC, TD, TSelect>> fields);
    }

    public interface IDbJoinable3<TA, TB, TC>
    {
        IDbJoinable4<TA, TB, TC, T> Join<T>(IDbQueryable<T> join,  Expression<Func<TA, TB, TC, T, bool>> condition);
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TA, TB, TC, TSelect>> fields);
    }
    
    public interface IDbJoinable2<TA, TB>
    {
        // Join 
        IDbJoinable3<TA, TB, T> Join<T>(IDbQueryable<T> join,  Expression<Func<TA, TB, T, bool>> condition);
        
        // Aggregation
        IDbQueryable<IDbAggregate<(TA, TB)>> GroupBy<TKey>(Expression<Func<TA, TB, TKey>> order);
        // Select fields
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TA, TB, TSelect>> fields);
        // Order
        IDbJoinable2<TA, TB> Order(Expression<Func<TA, TB>> order, OrderBy orderBy);
        // Where
        IDbJoinable2<TA, TB> Where(Expression<Func<TA, TB, bool>> predicate);
    }

    public interface IDbQueryFetchable<TSource>: IDbQueryable<TSource>
    {
        // execute this queryable
        TSource[] FetchMultiple();
        TSource FetchSingle();
    }
    
    public interface IDbQueryable<TSource>
    {
        // Joins
        IDbJoinable2<TSource, T> Join<T>(JoinType joinType, IDbQueryable<T> join,  Expression<Func<TSource, T, bool>> condition = null);
        // Aggregation
        IDbQueryable<IDbAggregate<T>> GroupBy<T>(Expression<Func<TSource, T>> order);
        // Select fields
        IDbQueryFetchable<TSelect> Select<TSelect>(Expression<Func<TSource, TSelect>> fields);
        IDbQueryFetchable<TSource> SelectAll();
        // Order
        IDbQueryable<TSource> Order(Expression<Func<TSource, TSource>> order, OrderBy orderBy);
        // Where
        IDbQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate);
        
        // Unions
        IDbQueryFetchable<TSource> Union(IDbQueryable<TSource> other);
        IDbQueryFetchable<TSource> UnionAll(IDbQueryable<TSource> other);
    }

    public interface IDbUpdatable<TTable>
    {
        IDbUpdatable<TTable> Update(Expression<Func<TTable>> field, Expression<Func<TTable>> value);
        IDbUpdatable<TTable> Where(Expression<Func<TTable, bool>> predicate);
        int Update();
    }
    
    public interface IDbTable<TTable>: IDbQueryable<TTable>
    {
        // Delete
        int Delete(Expression<Func<TTable, bool>> predicate);
        // Update
        IDbUpdatable<TTable> Update(Expression<Func<TTable>> field, Expression<Func<TTable>> value);
        // Insert
        int Insert(IEnumerable<TTable> entries);
        int Insert(TTable entry);
    }

    public abstract class DatabaseContext
    {
        public IDbQueryable<TFields> Const<TFields>(Func<TFields> fields)
        {
            return null;
        }
    }

    // ---------------------------

    public class Enquiries
    {
        public int Id { get; set; }
        public int UserId { get; set;  }
        
    }

    public class User
    {
        public int Id { get; set;  }
        public string Name { get; set; }
    }

    public class TopUp
    {
        public int Id { get; set; }
        public int UserId { get; set;  }
        public DateTime Added { get; set; }
        public int Count { get; set; }
    }

    public class TestDb : DatabaseContext
    {
        public DbTable<Enquiries> Enquiries { get; set;  }
        public DbTable<User> Users { get; set; }
        public DbTable<TopUp> Topups { get; set;  }

        public void Query()
        {
            Enquiries
                .GroupBy(e => e.UserId)
                .Select(e => new
                {
                    UserId = e.Key,
                    Used = e.Count(null)
                })
                .Join(JoinType.Inner, Topups, (e, t) => e.Used == t.Count)
                .Join(Users, (e, _, u) => e.UserId == u.Id)
                .Select((e, t, u) => new
                {
                    Used = e.Used + t.Count,
                    NAme = u.Name
                });
        }
        
    }

    public abstract class SqlSource
    {
        
    }
    
    public class SqlNoSource : SqlSource
    {
        
    }
    
    public class SqlTable : SqlSource
    {
        
    }

    public class SqlQuerySelect: SqlSource
    {
        public string[] Fields { get; set; }
        public SqlSource Source { get; set; }
        public string GroupBy { get; set; }
        public string Where { get; set; }
        public string Join { get; set; }
    }
    
    
    
    
}