using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace UnitTestProject1
{
    public class TransactionContainer : IDisposable
    {
        private readonly IEnumerable<DbContextTransaction> _transactions;

        public TransactionContainer(params DbContext[] databases)
        {
            _transactions = databases.Select(d => d.Database.BeginTransaction()).ToArray();
        }

        public void Commit()
        {
            foreach (var transaction in _transactions.Where(t => t != null))
            {
                transaction.Commit();
            }
        }

        public void Dispose()
        {
            foreach (var transaction in _transactions.Where(t => t != null))
            {
                transaction.Dispose();
            }
        }
    }
}