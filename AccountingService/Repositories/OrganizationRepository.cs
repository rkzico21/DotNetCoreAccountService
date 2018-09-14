namespace AccountingService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;
    public class OrganizationRepository : RepositoryBase<Organization>
    {
        private readonly AccountingDbContext dbContext;

        public OrganizationRepository(AccountingDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}