namespace AccountingService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;
    public class AccountRepository
    {
        private readonly AccountingDbContext dbContext;

        public AccountRepository(AccountingDbContext dbContext)
        {
            this.dbContext = dbContext;       
        }

        public Account Add(Account account) 
        {
            this.dbContext.Accounts.Add(account);
            this.dbContext.SaveChanges();
            return account;
        }
        
        public void Delete(int id)
        {
             var account = this.FindById(id);
             this.dbContext.Accounts.Remove(account);
             this.dbContext.SaveChanges();
        }

        public IEnumerable<Account> FindAll()
        {
            return this.dbContext.Accounts.ToList();
        }

        public Account FindById(int id)
        {
            return this.dbContext.Accounts.FirstOrDefault(a => a.Id == id);
        }
        
        public IEnumerable<Account> FindAll(int? organizationId, int? group, int? accountType)
        {
            var allAccounts = this.dbContext.Accounts.AsQueryable();
            
            if(group.HasValue)
            {
                allAccounts = allAccounts.Where(a=>a.GroupId == group);
            }
            
            if(accountType.HasValue)
            {
                allAccounts = allAccounts.Where(a=>a.AccountTypeId == accountType.Value);
            }
            
            return allAccounts.ToList();
        } 
    }
}