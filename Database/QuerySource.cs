using System;
using System.Linq.Expressions;

namespace codequery.Database
{
    public class QuerySource<T>
    {
        public PostSelectQuerySource<N> Select<N>(Expression<Func<T, N>> fields)
        {
            return new PostSelectQuerySource<N>();
        }

        public QuerySource<T> Where(Expression<Func<T, bool>> predicate)
        {
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
    }
    
    public class Database
    {
        // Constant source queries
        public PostSelectQuerySource<T> Select<T>(T fields)
        {
            return new PostSelectQuerySource<T>();
        }
    }

    public class MyDatabase : Database
    {
        public DatabaseTable<Station> Stations { get; set; }        
    }

    public class DatabaseTable<T> : QuerySource<T>
    {
        
    }

    public static class SematicsTest
    {
        public static void Run()
        {
            var db = new MyDatabase();

            // Select fields from source where ...
            // Select from (source) where ... fields 
            
            // // table source
            // db.From<Station>
            // // constant source
            // db.Select(10)
            // // SubQuery source
            // db.From(db.From<Station>)


            db.Stations
                .Where(s => s.FarmId == 10)
                .Select(s => s.Id)
                .FetchSingle();
            
            db.Stations
                .Where(s => s.Active)
                .Select(s => new {
                    Farm = s.FarmId,
                    Age = s.FarmId.Value
                })
                .FetchArray();
                
            db.Select(10)
                .FetchSingle();

            db.Select(new {
                a = 10,
                b = 20
            })
            .FetchSingle();
            
        }
    }

}