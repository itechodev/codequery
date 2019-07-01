namespace codequery.Database
{
    public class QuerySource
    {
        
    }

    public class Database
    {
        public DatabaseContant Constant { get; private set; }
    }

    public class MyDatabase : Database
    {
        public DatabaseTable<Station> Stations { get; set; }
    }

    public class DatabaseTable<T> : QuerySource
    {

    }

    public class DatabaseContant : QuerySource
    {

    }

    public static class SymaticsTest
    {
        public static void Run()
        {
            var db = new MyDatabase();
            
        }
    }
    
    // From table:
    // db.Station.
    // db.Station.
    
    // From consts
    // db.Select(10)
    // Constant.Select(new {a = 10, b = 20, c = 30}).



}