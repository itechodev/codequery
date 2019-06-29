using codequery.Expressions;

namespace codequery.Drivers
{
    public interface IDatabaseDriver
    {
        string GenerateSelect(SelectQuery query);
    }
}