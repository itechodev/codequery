using Xunit;

namespace CodeQuery
{
    public static class DebugCases
    {
        [Fact]
        public static void Simple()
        {
            var db = new TestDb();
            db.Initialize();
            var res = db.From<TopUp>()
                .Select(t => new {t.Added, t.Count})
                .FetchMultiple();

        }
    }
}