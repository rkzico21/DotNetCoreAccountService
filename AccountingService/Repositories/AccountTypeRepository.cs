namespace AccountingService.Repositories
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;
    using Microsoft.EntityFrameworkCore;

    public class AccountTypeRepository : RepositoryBase<AccountType>
    {
        private readonly AccountingDbContext dbContext;

        public AccountTypeRepository(AccountingDbContext dbContext)
            : base(dbContext)
        {
        
        }

        /*public AccountType Add(AccountType accountType) 
        {
            this.dbContext.AccountTypes.Add(accountType);
            this.dbContext.SaveChanges();
            return accountType;
        }
        
        public AccountType FindById(int id)
        {
            var queryble = this.GetAccountsTypeQueryble();
            return queryble.FirstOrDefault(a => a.Id == id);
        }
        */
        public IEnumerable<AccountType> FindAll(int? organizationId, int? group, bool includeAccounts = false)
        {
            var allAccountTypes = this.GetAccountsTypeQueryble(includeAccounts);
            
            if(group.HasValue)
            {
                allAccountTypes = allAccountTypes.Where(a=>a.GroupId == group);
            }
            
            return allAccountTypes.ToList();
        } 


        private IQueryable<AccountType> GetAccountsTypeQueryble(bool includeAccounts = false)
        {
             var queryble = this.dbContext.AccountTypes.AsQueryable();
             if(includeAccounts)
             {
                queryble = queryble.Include(t=>t.Accounts);
             }

             return queryble;
        }
    }
}