using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;

namespace CodeQuery
{ 
    /// <summary>
    /// Set Returning Functions
    /// https://www.postgresql.org/docs/9.5/functions-srf.html
    /// </summary>
    public static class DbFunctions
    {
        public static IDbQueryFetchable<int> GenerateSeries(int from, int to)
        {
            // var func = new SqlFunctionExpression(
            //     "generate_series",
            //     new SqlConstExpression(from, SqlColumnType.Int32),
            //     new SqlConstExpression(to, SqlColumnType.Int32)
            // );
            return null;
        }
    }
    
    public class DbSchema
    {
        private static Dictionary<Type, SqlTableSource> _definitions;

        private static SqlTableSource TableDefFromType(Type t)
        {
            if (_definitions == null)
            {
                throw new DbQueryBuilderException("Database schema not initialized. Did you forget to invoke the initialize method?");
            }
            if (!_definitions.ContainsKey(t))
            {
                throw new DbQueryBuilderException($"Cannot resolve type '{t.Name}' to a SQL table because it does not exists. Did you forget add a migration?");
            }
            return _definitions[t];
        }

        protected DbTableQuery<TTable> CreateTable<TTable>()
        {
            return new DbTableQuery<TTable>();
        }
            
        
        public IDbQueryable<T> From<T>() where T : DbTable
        {
            // return new DbTableQuery<T>(this, TableDefFromType(typeof(T)));
            throw new System.NotImplementedException();
        }
        
        public IDbQueryable<T> Select<T>(T fields)
        {
            throw new System.NotImplementedException();
        }
        
        
        
        public IDbQueryable<TFields> Const<TFields>(Func<TFields> fields)
        {
            return null;
        }
        
        public void Initialize()
        {
            // share cache between multiple instances of DatabaseContext
            // if (_definitions != null)
            // {
            //     return;
            // }
            //
            // // Read all DbTables from this assembly and convert into table definitions
            // _definitions = Assembly.GetEntryAssembly()
            //     ?.GetTypes()
            //     .Where(t => !t.IsInterface && typeof(DbTable).IsAssignableFrom(t))
            //     .ToDictionary(t => t, t => new SqlTableSource(t));
        }
    }
}