namespace AccountingService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;
    
    public class AccountGroupRepository : RepositoryBase<AccountGroup>
    {
        private readonly AccountingDbContext dbContext;

        public AccountGroupRepository(AccountingDbContext dbContext) :
            base(dbContext)
        {
            
        }
    }
}