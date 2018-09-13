namespace AccountingService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(AccountingDbContext dbContext)
               : base(dbContext)
        {
        }

        public User FindUserByEmail(string email) => this.DbContext.Users.FirstOrDefault(u => u.Email.Equals(email));


    }
}