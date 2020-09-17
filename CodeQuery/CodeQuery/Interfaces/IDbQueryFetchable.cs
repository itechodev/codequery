namespace CodeQuery.Interfaces
{
    public interface IDbQueryFetchable<TSource>: IDbQueryable<TSource>
    {
        // execute this queryable
        TSource[] FetchMultiple();
        TSource FetchSingle();
    }
}