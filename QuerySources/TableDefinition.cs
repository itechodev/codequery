namespace codequery.QuerySources
{
    public class TableDefinition
    {
        public string Name { get; set; }
        public ColumnDefinition[] Columns { get; set; }
        public FieldProp[] FieldsAndProps { get; internal set; }
    }
}