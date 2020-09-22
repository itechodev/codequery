using System;
using CodeQuery.Definitions;
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
                .Select(t => new
                {
                    t.Added, 
                    Count =  t.Count, 
                    Now = DateTime.Now,
                    Count2 = t.Count * 10,
                    AdedAsString = t.Added.ToShortDateString()
                })
                .FetchMultiple();

        }
        
    }
}