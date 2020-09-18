namespace CodeQuery.Interfaces
{
    public interface IDbAggregate<out TKey, TSource> : IDbQueryable<TSource> 
    {
        TKey Key { get; }
    }
}