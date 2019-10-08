using codequery.QuerySources;

namespace codequery.Expressions
{

    public class SqlTableSource : SqlQuerySource
    {
    
        public SqlTableSource(TableDefinition def, string alias) : base(def.Columns, alias)
        {
            Name = def.Name;
        }

        public string Name { get; }
    }

    public class SqlReferencedField
    {
        public SqlQuerySource Source { get; set; }
        public ColumnDefinition Column { get; set; }
    }

    public enum FieldType
    {
        Int,
        String,
        Char,
        Double,
        Binary,
        Bool
    }

   

}
