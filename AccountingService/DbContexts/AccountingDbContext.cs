namespace AccountingService.DbContexts 
{
    using System;
    using AccountingService.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Collections;
    using System.Collections.Generic;

    public class AccountingDbContext : DbContext
    {
        
        private readonly Dictionary<Type, Object> DbSets;
        public AccountingDbContext(DbContextOptions<AccountingDbContext> options)
            : base(options)
        {
            this.DbSets = new Dictionary<Type, Object>();
            this.DbSets.Add(typeof(Account), Accounts);
            this.DbSets.Add(typeof(AccountGroup), AccountGroups);
            this.DbSets.Add(typeof(AccountType), AccountTypes);
            this.DbSets.Add(typeof(Transaction), Transactions);
        }



        public DbSet<T> GetDbSet<T>(Type type) where T: EntityBase
        {
              return  (DbSet<T>)DbSets[type] ;  
        }
 
        public DbSet<Account> Accounts { get; set;}

        public DbSet<AccountGroup> AccountGroups { get; set;}
        
        public DbSet<AccountType> AccountTypes { get; set;}

        public DbSet<Organization> Organizations { get; set;}
        
        public DbSet<Transaction> Transactions { get; set;}



       protected override void  OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.Entity<AccountType>().HasMany(t => t.Accounts );

           modelBuilder.Entity<AccountGroup>().HasData(
               new AccountGroup{ Id=1, Name = "Assets"}, 
               new AccountGroup{ Id=2, Name = "Liabilities"}, 
               new AccountGroup{ Id=3, Name = "Equity"});

           modelBuilder.Entity<AccountType>().HasData(
               new AccountType{Id = 1, Name = "Cash/Bank", GroupId=1}, 
               new AccountType{Id = 2, Name = "Money in Transit", GroupId=1},
               new AccountType{Id = 3, Name = "Payments from Sales", GroupId=1},
               
               new AccountType{Id = 4, Name = "Credit Card", GroupId=2},
               new AccountType{Id = 5, Name = "Loan and Line of Credit", GroupId=2},
               new AccountType{Id = 6, Name = "Taxes", GroupId=2},
               
               new AccountType{Id = 7, Name = "Own Contribution", GroupId=3},
               new AccountType{Id = 8, Name = "Drawing", GroupId=3}
               );


            modelBuilder.Entity<Account>().HasData(
                new Account{Id = 1, Name = "Cash at Hand", GroupId=1, AccountTypeId = 1 } 
            );

            modelBuilder.Entity<Organization>().HasData(
                new Organization{Id = 1, Name = "Organization 1" } 
            );
       }

    }

}