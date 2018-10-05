namespace AccountingService.DbContexts 
{
    using System;
    using AccountingService.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class AccountingDbContext : DbContext
    {
        
        private readonly Dictionary<Type, Object> DbSets;
        private readonly PasswordHasher<User> passwordHasher;

        public AccountingDbContext(DbContextOptions<AccountingDbContext> options, PasswordHasher<User> passwordHasher)
            : base(options)
        {
            this.DbSets = new Dictionary<Type, Object>();
            this.DbSets.Add(typeof(Account), Accounts);
            this.DbSets.Add(typeof(AccountGroup), AccountGroups);
            this.DbSets.Add(typeof(AccountType), AccountTypes);
            this.DbSets.Add(typeof(Transaction), Transactions);
            this.DbSets.Add(typeof(Organization), Organizations);
            this.DbSets.Add(typeof(User), Users);
            this.passwordHasher = passwordHasher;
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

        public DbSet<User> Users { get; set; }
        
        
        protected override void  OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<AccountType>().HasMany(t => t.Accounts);
           modelBuilder.Entity<User>().HasOne(u=>u.Organization);
           modelBuilder.Entity<Transaction>().HasOne(t=>t.Account);
           modelBuilder.Entity<JournalTransaction>().HasBaseType<Transaction>();
           
           modelBuilder.Entity<Crebit>().HasBaseType<TransactionItem>().HasOne(t=>t.Transaction);
           modelBuilder.Entity<Debit>().HasBaseType<TransactionItem>().HasOne(t=>t.Transaction);




            modelBuilder.Entity<TransactionItem>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

            
          
           var accountGroup1=  new AccountGroup{ Id = Guid.NewGuid(),  Name = "Assets"};
           var accountGroup2 = new AccountGroup{ Id = Guid.NewGuid(), Name = "Liabilities"};
           var accountGroup3 =  new AccountGroup{  Id = Guid.NewGuid(), Name = "Equity"};
           modelBuilder.Entity<AccountGroup>().HasData(accountGroup1, accountGroup2, accountGroup3);
           

           var accountType1 = new AccountType{ Id = Guid.NewGuid(), Name = "Cash/Bank", GroupId=accountGroup1.Id}; 
           var accountType2 = new AccountType{ Id = Guid.NewGuid(), Name = "Money in Transit", GroupId=accountGroup1.Id};
           var accountType3 =    new AccountType{ Id = Guid.NewGuid(), Name = "Payments from Sales", GroupId=accountGroup1.Id};
               
            var accountType4 =   new AccountType{ Id = Guid.NewGuid(), Name = "Credit Card", GroupId=accountGroup2.Id};
            var accountType5 =   new AccountType{ Id = Guid.NewGuid(), Name = "Loan and Line of Credit", GroupId=accountGroup2.Id};
            var accountType6 =   new AccountType{ Id = Guid.NewGuid(), Name = "Taxes", GroupId=accountGroup2.Id};
               
            var accountType7 =   new AccountType{ Id = Guid.NewGuid(), Name = "Own Contribution", GroupId=accountGroup3.Id};
            var accountType8 =   new AccountType{ Id = Guid.NewGuid(), Name = "Drawing", GroupId=accountGroup3.Id};
           
           modelBuilder.Entity<AccountType>().HasData(accountType1,
                accountType2, accountType3, accountType4, accountType5, accountType6, accountType7, accountType8
            );



            var organization1 =    new Organization{ Id = Guid.NewGuid(), Name = "Organization 1"} ;
            var organization2 =    new Organization{ Id = Guid.NewGuid(), Name = "Organization 2"} ;
            var organization3 =    new Organization{ Id = Guid.NewGuid(), Name = "Organization 3"} ;

           modelBuilder.Entity<Organization>().HasData(organization1, organization2, organization3);
           
            var account1 = new Account{ Id = Guid.NewGuid(),  Name = "Cash at Hand", GroupId= accountGroup1.Id, AccountTypeId = accountType1.Id, OrganizationId= organization1.Id }; 
           
            modelBuilder.Entity<Account>().HasData(account1);

            
            
           /*modelBuilder.Entity<JournalTransaction>().HasData(
                new JournalTransaction{ Id = 3, OrganizationId = 1, AccountId = 1,  TransactionDate = DateTime.Now, Amount =100}
            );
            
            modelBuilder.Entity<TransactionItem>().HasData(
                new TransactionItem {Id = 1, TransactionType = "credit", Amount = 100, TransactionId = 3, AccountId = 1, OrganizationId=1 },
                new TransactionItem {Id = 2, TransactionType = "debit", Amount = 100, TransactionId = 3, AccountId =1, OrganizationId=1}   
            );*/

            

            
            var user1 = new User{ Id = Guid.NewGuid(),  Name = "User 1", Email="user1@Organization1.com", Password = "123456", OrganizationId = organization1.Id};
            var user2 = new User{ Id = Guid.NewGuid(), Name = "User 2", Email="user2@Organization2.com", Password = "123456", OrganizationId = organization2.Id};    
            var user3 = new User{ Id = Guid.NewGuid(), Name = "User 3", Email="user3@Organization3.com", Password = "123456", OrganizationId = organization3.Id};  
            
            user1.Password =  passwordHasher.HashPassword(user1, user1.Password);
            user2.Password =  passwordHasher.HashPassword(user1, user2.Password);
            user3.Password =  passwordHasher.HashPassword(user1, user3.Password);
            
            modelBuilder.Entity<User>().HasData(user1,user2,user3);
        }

    }

}