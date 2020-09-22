using System;
using System.Collections.Generic;
using System.Reflection;
using CodeQuery.Attributes;

namespace CodeQuery.Definitions
{
    
    public class SqlTableDefinition
    {
        // Table name as in the database
        public string Name { get; }
        public List<SqlColumnDefinition> Columns { get;  }

        public SqlTableDefinition(string name)
        {
            Name = name;
            Columns = new List<SqlColumnDefinition>();
        }

        public SqlTableDefinition(Type t)
        {
            var customName = t.GetCustomAttribute<TableAttribute>();
            var tableName = customName?.Name ?? t.Name;
            
            Columns = new List<SqlColumnDefinition>();
            foreach (var prop in t.GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    Columns.Add(ToSqlColumn(prop));
                }
            }
        }

        private SqlColumnDefinition ToSqlColumn(PropertyInfo info)
        {
            // override any if type is overwrote
            var colAttr = info.GetCustomAttribute<ColumnAttribute>();
            var type = colAttr?.ResolvedType ?? DefinitionHelper.InferType(info.PropertyType);

            var colDef = new SqlColumnDefinition(this, colAttr?.Name ?? info.Name, type);
            // colDef.SetFlags();
            // colDef.SetPrecision();
            // colDef.SetForeignKey();

            return colDef;
        }

        
    }
}