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
            if (!_definitions.ContainsKey(t))
            {
                throw new ArgumentException($"Cannot resolve type '{t.Name}' to a SQL table because it does not exists. Did you forget add a migration?");
            }
            return _definitions[t];
        }
        
        public IDbQueryable<T> From<T>() where T : DbTable
        {
            var source = new SqlTableSource(TableDefFromType(typeof(T)));
            return new DbQuery<T>(this, new SqlQuerySelect(source));
        }
        
        public IDbQueryable<TFields> Const<TFields>(Func<TFields> fields)
        {
            return null;
        }

        // extract the IDTable type if derived from generic IDbTable<T> 
        private static Type GetTableType(Type type)
        {
            var interfaces = type.GetInterfaces();
            if (interfaces?.FirstOrDefault() == typeof(IDbTable<>))
            {
                return type.GenericTypeArguments.FirstOrDefault();
            }

            return null;
        }

        public void PopulateDefinitions()
        {
            // share cache between multiple instances of DatabaseContext
            if (_definitions != null)
            {
                return;
            }
            
            // Read all DbTables from this instance and convert this into table definitions
            _definitions = new Dictionary<Type, SqlTableDefinition>();
            foreach (var prop in GetType().GetProperties(BindingFlags.Public))
            {
                if (!prop.CanRead)
                {
                    continue;
                }

                // extract the first generic type from IDbTable<..>
                var tableType = GetTableType(prop.PropertyType);
                if (tableType != null)
                {
                    _definitions[tableType] = new SqlTableDefinition(tableType);
                }
            }
        }
    }
}