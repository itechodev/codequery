using System;
using System.Collections.Generic;
using System.Reflection;
using CodeQuery.Attributes;
using Xunit.Sdk;

namespace CodeQuery.Definitions
{
    
    public class SqlTableDefinition : SqlSource
    {
        // Table name as in the database
        public string Name { get; }
        
        // public SqlTableDefinition(Type t): base(null, t)
        // {
        //     var customName = t.GetCustomAttribute<TableAttribute>();
        //     var tableName = customName?.Name ?? t.Name;
        //     
        //     Columns = new List<SqlColumnDefinition>();
        //     foreach (var prop in t.GetProperties())
        //     {
        //         if (prop.CanRead && prop.CanWrite)
        //         {
        //             Columns.Add(ToSqlColumn(prop));
        //         }
        //     }
        // }

        private SqlColumnDefinition ToSqlColumn(PropertyInfo info)
        {
            // // override any if type is overwrote
            // var colAttr = info.GetCustomAttribute<ColumnAttribute>();
            // var type = colAttr?.ResolvedType ?? DefinitionHelper.InferType(info.PropertyType);
            //
            // var colDef = new SqlColumnDefinition(this, colAttr?.Name ?? info.Name, type);
            // // colDef.SetFlags();
            // // colDef.SetPrecision();
            // // colDef.SetForeignKey();
            //
            // return colDef;

            throw new NotImplementedException();
        }

        
    }
}