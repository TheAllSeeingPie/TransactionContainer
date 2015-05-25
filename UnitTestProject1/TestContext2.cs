using System.Data.Entity;

namespace UnitTestProject1
{
    public class TestContext2 : DbContext
    {
        public TestContext2()
            : base("Server=.;Database=TestContext2;Trusted_Connection=True;Enlist=false")
        {
        }

        public DbSet<ASomethingElse> SomethingEleses { get; set; } 
    }
}