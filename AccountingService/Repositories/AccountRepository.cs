namespace AccountingService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;
    public class AccountRepository : RepositoryBase<Account>
    {
        
        public AccountRepository(AccountingDbContext dbContext) 
                : base(dbContext)
        {
                   
        }
        
        public IEnumerable<Account> FindAll(string organizationId, string group, string accountType)
        {
            var allAccounts = this.DbContext.Accounts.AsQueryable();
            
            if(!string.IsNullOrWhiteSpace(group))
            {
                allAccounts = allAccounts.Where(a=>a.GroupId.ToString() == group);
            }
            
            if(!string.IsNullOrWhiteSpace(accountType))
            {
                allAccounts = allAccounts.Where(a=>a.AccountTypeId.ToString() == accountType);
            }

            if(!string.IsNullOrWhiteSpace(organizationId))
            {
                allAccounts = allAccounts.Where(a => a.OrganizationId.ToString() == organizationId);
            }
            
            return allAccounts.AsEnumerable();
        } 
    }
}