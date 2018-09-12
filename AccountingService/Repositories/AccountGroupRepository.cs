using System;
using System.Collections.Generic;
using System.Linq;
using AccountingService.DbContexts;
using AccountingService.Entities;

namespace AccountingService.Repositories
{
    public class AccountGroupRepository : RepositoryBase<AccountGroup>
    {
        private readonly AccountingDbContext dbContext;

        public AccountGroupRepository(AccountingDbContext dbContext) :
            base(dbContext)
        {
            
        }

        /* 
        public AccountGroup Add(AccountGroup accountGroup) 
        {
            this.dbContext.AccountGroups.Add(accountGroup);
            this.dbContext.SaveChanges();
            return accountGroup;
        }
        
        
        
        public AccountGroup FindById(int id)
        {
            return this.dbContext.AccountGroups.FirstOrDefault(a => a.Id == id);
        }*/

        public IEnumerable<AccountGroup> FindAll()
        {
            return this.DbContext.AccountGroups.AsEnumerable();
        }
    }
}