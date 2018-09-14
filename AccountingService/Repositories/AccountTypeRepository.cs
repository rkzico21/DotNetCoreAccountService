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

       
        public IEnumerable<AccountType> FindAll(int? group, bool includeAccounts, int? organizationId)
        {
            var allAccountTypes = this.GetAccountsTypeQueryble(includeAccounts);
            
            if(group.HasValue)
            {
                allAccountTypes = allAccountTypes.Where(a=>a.GroupId == group);
            }
            
            var types = allAccountTypes.AsEnumerable();

            if(organizationId.HasValue)
            {
                foreach(var type in types)
                {
                    type.Accounts = type.Accounts.Where(a=>a.OrganizationId == organizationId.Value).ToList();
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