using System.Data.Entity;

namespace UnitTestProject1
{
    public class TestContext1 : DbContext
    {
        public TestContext1()
            : base("Server=.;Database=TestContext1;Trusted_Connection=True;Enlist=false")
        {
        }

        public DbSet<ASomething> Somethings { get; set; } 
    }
}