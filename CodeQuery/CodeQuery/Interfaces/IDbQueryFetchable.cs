namespace CodeQuery.Interfaces
{
    public interface IDbQueryFetchable<TSource>: IDbQueryable<TSource>
    {
        // execute this queryable
        TSource[] FetchMultiple(int? skip = null, int? take = null);
        TSource FetchSingle();
    }
}