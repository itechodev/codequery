namespace codequery.QuerySources
{
    public class ResultQuerySource<T>
    {
        public ResultQuerySource<T> Union(T fields)
        {
            return null;
        }

        public ResultQuerySource<T> UnionAll(T fields)
        {
            return null;
        }


        public T FetchSingle()
        {
            return default(T);
        }

        public T[] FetchArray()
        {
            return null;
        }
    }
}