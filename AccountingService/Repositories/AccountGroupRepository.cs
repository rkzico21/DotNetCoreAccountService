using System;
using System.Collections.Generic;
using System.Linq;
using AccountingService.DbContexts;
using AccountingService.Entities;

namespace AccountingService.Repositories
{
    public class AccountGroupRepository
    {
        private readonly AccountingDbContext dbContext;

        public AccountGroupRepository(AccountingDbContext dbContext)
        {
            this.dbContext = dbContext;       
        }

        public AccountGroup Add(AccountGroup accountGroup) 
        {
            this.dbContext.AccountGroups.Add(accountGroup);
            this.dbContext.SaveChanges();
            return accountGroup;
        }
        
        public IEnumerable<AccountGroup> FindAll()
        {
            return this.dbContext.AccountGroups.ToList();
        }
        
        public AccountGroup FindById(int id)
        {
            return this.dbContext.AccountGroups.FirstOrDefault(a => a.Id == id);
        }
    }
}