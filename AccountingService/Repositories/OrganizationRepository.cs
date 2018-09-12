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
        /* 
        public IEnumerable<Organization> FindAll()
        {
            return this.dbContext.Organizations.AsEnumerable();
        }

        public Organization FindById(int id)
        {
            return this.dbContext.Organizations.FirstOrDefault(o=> o.Id == id );
        }

        public Organization Add(Organization organization)
        {
            this.dbContext.Organizations.Add(organization);
            this.dbContext.SaveChanges();
            return organization;

        }

       

        public void Delete(int id)
        {
            var organization = this.dbContext.Organizations.FirstOrDefault(o=> o.Id == id);
            if(organization != null)
            {
                this.dbContext.Organizations.Remove(organization);
                this.dbContext.SaveChanges();
            }
        }*/

        public Organization Update(Organization organization)
        {
            throw new NotImplementedException();
        }
    }
}