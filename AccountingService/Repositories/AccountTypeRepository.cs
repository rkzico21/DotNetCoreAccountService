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
        public AccountTypeRepository(AccountingDbContext dbContext)
            : base(dbContext)
        {
        
        }

        public IEnumerable<AccountType> FindAll(string group, bool includeAccounts, string organizationId)
        {
            var allAccountTypes = this.GetAccountsTypeQueryble(includeAccounts);
            
            if(!string.IsNullOrWhiteSpace(group))
            {
                allAccountTypes = allAccountTypes.Where(a=>a.GroupId.ToString() == group);
            }
            
            var types = allAccountTypes.AsEnumerable();

            if(!string.IsNullOrWhiteSpace(organizationId))
            {
                foreach(var type in types)
                {
                    type.Accounts = type.Accounts.Where(a=>a.OrganizationId.ToString() == organizationId).ToList();
                }
            }

            return types;
        } 


        private IQueryable<AccountType> GetAccountsTypeQueryble(bool includeAccounts)
        {
             var queryble = this.DbContext.AccountTypes.AsQueryable();
             if(includeAccounts)
             {
                queryble = queryble.Include(t=>t.Accounts);
             }

             return queryble;
        }
    }
}