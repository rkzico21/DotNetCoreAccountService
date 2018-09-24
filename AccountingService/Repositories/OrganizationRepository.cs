namespace AccountingService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;
    using Microsoft.EntityFrameworkCore;

    public class OrganizationRepository : RepositoryBase<Organization>
    {
        public OrganizationRepository(AccountingDbContext dbContext) 
            : base(dbContext)
        {
            
        }

        public Organization Add(Organization entity, string ownerEmail)
        {
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                var organization = base.Add(entity);
                var user = this.DbContext.Users.FirstOrDefault(u=> u.Email == ownerEmail);
                user.Organization = organization;
                DbContext.Entry(user).State = EntityState.Modified;
                DbContext.SaveChanges();
                transaction.Commit();
            }

            return entity;
        }
    }
}