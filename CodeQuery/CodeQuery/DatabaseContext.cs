using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;

namespace CodeQuery
{
    public abstract class DatabaseContext
    {
        private static Dictionary<Type, SqlTableDefinition> _definitions;
        
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

        protected void Initialize()
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