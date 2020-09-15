namespace codequery.QuerySources
{
    public class DatabaseTable<T> : QuerySource<T>
    {
        public DatabaseTable<T> OrderBy()
        {
            return this;
        }
    }
}