using CodeQuery.Interfaces;

namespace CodeQuery
{
    public class DbTableBuilder
    {
        public void AddColumn()
        {
        }
    }
    
    // Only used to identify and extend tables 
    // DbTableExtensions uses this as generic constraint
    public class DbTable: IDbSource
    {
        public virtual void OnConfigure(DbTableBuilder builder)
        {
            
        }
    }
}