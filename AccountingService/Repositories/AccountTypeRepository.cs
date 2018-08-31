namespace AccountingService.Repositories
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;

    public class AccountTypeRepository
    {
        private readonly AccountingDbContext dbContext;

        public AccountTypeRepository(AccountingDbContext dbContext)
        {
            this.dbContext = dbContext;       
        }

        public AccountType Add(AccountType accountType) 
        {
            this.dbContext.AccountTypes.Add(accountType);
            this.dbContext.SaveChanges();
            return accountType;
        }
        
        public IEnumerable<AccountType> FindAll()
        {
            return this.dbContext.AccountTypes.ToList();
        }

        public AccountType FindById(int id)
        {
            return this.dbContext.AccountTypes.FirstOrDefault(a => a.Id == id);
        }
        
        public IEnumerable<AccountType> FindAll(int? organizationId, int? group)
        {
            var allAccountTypes = this.dbContext.AccountTypes.AsQueryable();
            
            if(group.HasValue)
            {
                allAccountTypes = allAccountTypes.Where(a=>a.GroupId == a.GroupId);
            }
            
            return allAccountTypes.ToList();
        } 
    }
}