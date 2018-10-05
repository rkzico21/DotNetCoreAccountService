namespace AccountingService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;
    public class TransactionRepository : RepositoryBase<Transaction>
    {
        public TransactionRepository(AccountingDbContext dbContext)
               : base(dbContext)
        {
        }
        public IEnumerable<Transaction> FindAll(string organizationId)
        {
            var allTransactions = this.DbContext.Transactions.AsQueryable();
            
            if(!string.IsNullOrWhiteSpace(organizationId))
            {
               allTransactions = allTransactions.Where(t => t.OrganizationId.ToString() == organizationId);
            }
            
             return allTransactions.AsEnumerable();
        }

        public void LoadItems(JournalTransaction transaction)
        {
            this.DbContext.Entry(transaction).Collection(t=>t.Debits).Load();
            
            this.DbContext.Entry(transaction).Collection(t=>t.Credits).Load();
        }  
    }
}