using CodeQuery.Interfaces;

namespace CodeQuery
{
    // Only used to identify and extend tables 
    // DbTableExtensions uses this as generic constraint
    public interface DbTable: IDbSource
    {
    }
}