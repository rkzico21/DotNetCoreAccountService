namespace AccountingService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.Entities;
    using AccountingService.Repositories;

    public class AccountService
    {
        private AccountRepository repository;
        
        public AccountService(AccountRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Account>  GetAccounts ()
        {
             return repository.FindAll();    
        }

        public IEnumerable<Account> GetAccounts(int? organizationId, int? group, int? accountType)
        {
             return repository.FindAll(organizationId, group, accountType);
        }

        public Account CreateAccount(Account account)
        {
            return this.repository.Add(account);
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public Account GetAccount(int id)
        {
            return this.repository.FindById(id);
        }
    }
}