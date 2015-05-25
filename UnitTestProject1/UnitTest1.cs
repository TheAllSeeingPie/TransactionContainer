using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void TestInitialize()
        {
            using (var testContext1 = new TestContext1())
            using (var testContext2 = new TestContext2())
            {
                testContext1.Somethings.RemoveRange(testContext1.Somethings.ToArray());
                testContext1.SaveChanges();

                testContext2.SomethingEleses.RemoveRange(testContext2.SomethingEleses.ToArray());
                testContext2.SaveChanges();
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            const string conn1 = "Server=.;Database=TestContext1;Trusted_Connection=True;Enlist=false";
            const string conn2 = "Server=.;Database=TestContext2;Trusted_Connection=True;Enlist=false";

            const string somethings = "Select count(*) from ASomethings";
            const string somethingElses = "Select count(*) from ASomethingElses";

            using (var testContext1 = new TestContext1())
            using (var testContext2 = new TestContext2())
            {
                using (var transactionContainer = new TransactionContainer(testContext1, testContext2))
                {
                    testContext1.Somethings.Add(new ASomething { Name = "Bob" });
                    testContext1.SaveChanges();

                    testContext2.SomethingEleses.Add(new ASomethingElse { Total = 1 });
                    testContext2.SaveChanges();

                    Assert.AreEqual(0, GetRowCount(conn1, somethings));
                    Assert.AreEqual(0,  GetRowCount(conn2, somethingElses));

                    transactionContainer.Commit();

                    Assert.AreEqual(1, GetRowCount(conn1, somethings));
                    Assert.AreEqual(1, GetRowCount(conn2, somethingElses));
                }
            }
        }

        private static int GetRowCount(string connString, string query)
        {
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}
