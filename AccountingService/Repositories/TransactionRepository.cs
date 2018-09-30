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
        public IEnumerable<Transaction> FindAll(int? organizationId)
        {
            var allTransactions = this.DbContext.Transactions.AsQueryable();
            
            if(organizationId.HasValue)
            {
               allTransactions = allTransactions.Where(t => t.OrganizationId == organizationId.Value);
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