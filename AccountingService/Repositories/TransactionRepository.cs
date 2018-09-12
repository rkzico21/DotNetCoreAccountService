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

        /*public Transaction Add(Transaction transaction) 
        {
            this.dbContext.Transactions.Add(transaction);
            this.dbContext.SaveChanges();
            return transaction;
        }
        
        public void Delete(int id)
        {
             var transaction = this.FindById(id);
             if(transaction != null)
             {
                this.dbContext.Transactions.Remove(transaction);
                this.dbContext.SaveChanges();
             }
        }
        
        public Transaction FindById(int id)
        {
            return this.dbContext.Transactions.FirstOrDefault(a => a.Id == id);
        }
        */

        public IEnumerable<Transaction> FindAll(int? organizationId)
        {
            var allTransactions = this.dbContext.Transactions.AsQueryable();
            
            if(organizationId.HasValue)
            {
            
            }
            
<<<<<<< HEAD
=======
            
>>>>>>> 43dcf2b3ce1251bd9774ddf710fb4b77a0808073
            return allTransactions.AsEnumerable();
        } 
    }
}