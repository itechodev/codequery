namespace CodeQuery.Definitions
{
    public class SqlColumnForeignKey
    {
        public SqlColumnDefinition Destination { get; }
        public SqlColumnAction OnDelete { get; }
        public SqlColumnAction OnUpdate { get; }

        public SqlColumnForeignKey(SqlColumnDefinition destination, SqlColumnAction onDelete, SqlColumnAction onUpdate)
        {
            Destination = destination;
            OnDelete = onDelete;
            OnUpdate = onUpdate;
        }
    }
}