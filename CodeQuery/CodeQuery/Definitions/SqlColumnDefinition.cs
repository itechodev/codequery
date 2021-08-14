namespace CodeQuery.Definitions
{
    public class SqlColumnDefinition
    {
        public SqlColumnDefinition(SqlTableSource owner, string name, SqlColumnType type)
        {
            Owner = owner;
            Name = name;
            Type = type;
        }

        public void SetPrecision(int length, int precision)
        {
            Length = length;
            Precision = precision;
        }

        public void SetFlags(bool identity, bool primaryKey, bool nullable)
        {
            Identity = identity;
            PrimaryKey = primaryKey;
            Nullable = nullable;
        }

        public void SetForeignKey(SqlColumnDefinition fk)
        {
            ForeignKey = fk;
        }
        

        public SqlTableSource Owner { get; }
        public string Name { get; }
        public SqlColumnType Type { get; }
        public object DefaultValue { get; }
        
        public int? Length { get; private set; }
        public int? Precision { get; private set;  }
        
        // fro postgres 10 generate as identity 
        public bool Identity { get; private set; }
        
        // constraints
        public bool Nullable { get; private set; }
        public bool PrimaryKey { get; private set; }
        public SqlColumnDefinition ForeignKey { get; private set; }
        
        // Indexes to be added 
    }
}