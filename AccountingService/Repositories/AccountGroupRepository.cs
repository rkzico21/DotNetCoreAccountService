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
        
<<<<<<< HEAD
        
=======
        public IEnumerable<AccountGroup> FindAll()
        {
            return this.dbContext.AccountGroups.AsEnumerable();
        }
>>>>>>> 43dcf2b3ce1251bd9774ddf710fb4b77a0808073
        
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