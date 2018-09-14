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
        
        public IEnumerable<Account> FindAll(int? organizationId, int? group, int? accountType)
        {
            var allAccounts = this.DbContext.Accounts.AsQueryable();
            
            if(group.HasValue)
            {
                allAccounts = allAccounts.Where(a=>a.GroupId == group);
            }
            
            if(accountType.HasValue)
            {
                allAccounts = allAccounts.Where(a=>a.AccountTypeId == accountType.Value);
            }

            if(organizationId.HasValue)
            {
                allAccounts = allAccounts.Where(a => a.OrganizationId == organizationId.Value);
            }
            
            return allAccounts.AsEnumerable();
        } 
    }
}