using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;

namespace CodeQuery
{
    public abstract class DbSchema
    {
        private static Dictionary<Type, SqlTableDefinition> _definitions;

        private SqlTableDefinition TableDefFromType(Type t)
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
        
        public IDbQueryable<T> From<T>() where T : DbTable
        {
            return new DbTableQuery<T>(this, TableDefFromType(typeof(T)));
        }
        
        public IDbQueryable<TFields> Const<TFields>(Func<TFields> fields)
        {
            return null;
        }
        
        public void Initialize()
        {
            // share cache between multiple instances of DatabaseContext
            if (_definitions != null)
            {
                return;
            }
            
            // Read all DbTables from this assembly and convert into table definitions
            _definitions = new Dictionary<Type, SqlTableDefinition>();
            var tables = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsInterface && typeof(DbTable).IsAssignableFrom(t));
                
            foreach (var table in tables)
            {
                _definitions[table] = new SqlTableDefinition(table);
                
            }
        }
    }
}