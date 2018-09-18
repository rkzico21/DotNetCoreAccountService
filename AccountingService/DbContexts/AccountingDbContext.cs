namespace AccountingService.DbContexts 
{
    using System;
    using AccountingService.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

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
           modelBuilder.Entity<TransactionItem>().HasOne(i => i.Transaction).
                    WithMany(t=>t.Items).HasForeignKey(t=>t.TransactionId);

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


            modelBuilder.Entity<Organization>().HasData(
                new Organization{Id = 1, Name = "Organization 1"} 
            );

            modelBuilder.Entity<Organization>().HasData(
                new Organization{Id = 2, Name = "Organization 2" } 
            );


            
            modelBuilder.Entity<Organization>().HasData(
                new Organization{Id = 3, Name = "Organization 3"} 
            );


            modelBuilder.Entity<Account>().HasData(
                new Account{Id = 1, Name = "Cash at Hand", GroupId=1, AccountTypeId = 1, OrganizationId=1 } 
            );

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction{ Id=1, AccountId= 1, OrganizationId = 1, TransactionTypeId =1, TransactionDate = DateTime.Now }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account{Id = 2, Name = "Cash at Bank", GroupId=1, AccountTypeId = 1 ,OrganizationId=2 } 
            );

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction{ Id=2, AccountId= 2, OrganizationId = 2, TransactionTypeId =1 ,TransactionDate = DateTime.Now }
            );

            
            modelBuilder.Entity<JournalTransaction>().HasData(
                new JournalTransaction{ Id = 3, OrganizationId = 1, AccountId = 1,  TransactionDate = DateTime.Now}
            );
            
            modelBuilder.Entity<TransactionItem>().HasData(
                new TransactionItem {Id = 1, TransactionType = "credit", Amount = 100, TransactionId = 3 },
                new TransactionItem {Id = 2, TransactionType = "debit", Amount = 100, TransactionId = 3}   
            );

            

            
            var user1 = new User{Id =1, Name = "User 1", Email="user1@Organization1.com", Password = "123456", OrganizationId=1};
            var user2 = new User{Id =2, Name = "User 2", Email="user2@Organization2.com", Password = "123456", OrganizationId=2};    
            var user3 = new User{Id =3, Name = "User 3", Email="user3@Organization3.com", Password = "123456", OrganizationId=3};  
            
            user1.Password =  passwordHasher.HashPassword(user1, user1.Password);
            user2.Password =  passwordHasher.HashPassword(user1, user2.Password);
            user3.Password =  passwordHasher.HashPassword(user1, user3.Password);
            
            modelBuilder.Entity<User>().HasData(user1,user2,user3);
        }

    }

}