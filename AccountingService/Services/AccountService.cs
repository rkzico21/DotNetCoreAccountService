namespace AccountingService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.Entities;
    using AccountingService.Exceptions;
    using AccountingService.Repositories;
    using Microsoft.Extensions.Logging;

    public class AccountService
    {
        private readonly AccountRepository repository;
        private readonly AccountGroupRepository accountGroupRepository;

        private readonly AccountTypeRepository accountTypeRepository;

        private readonly ILogger logger;
        
        public AccountService(AccountRepository repository, AccountGroupRepository accountGroupRepository, AccountTypeRepository accountTypeRepository, ILogger<AccountService> logger)
        {
            this.repository = repository;
            this.accountGroupRepository = accountGroupRepository;
            this.accountTypeRepository = accountTypeRepository;
            this.logger = logger;
        }

        public IEnumerable<Account> GetAccounts(int? organizationId, int? group, int? accountType)
        {
             return repository.FindAll(organizationId, group, accountType);
        }

        public Account CreateAccount(Account account)
        {
            var accountType = this.accountTypeRepository.FindById(account.AccountTypeId.Value);
            if(accountType == null)
            {
                throw new ResourceNotFoundException($"Account Type with id:{account.AccountTypeId.Value} does not exist"); 
            }
            
            account.GroupId = accountType.GroupId; //make sure correct group id has been assigned.
            return this.repository.Add(account);
        }
        
        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public Account GetAccount(int id)
        {
            var account = this.repository.FindById(id);
            if(account == null)
            {
                var message = $"Account with id: {id} not found";
                logger.LogWarning(message);
                throw new ResourceNotFoundException(message); 
            }

            return account;
        }

        public IEnumerable<AccountGroup> GetGroups()
        {
            return this.accountGroupRepository.FindAll();
        }

        public IEnumerable<AccountType> GetAccountTypes(int? organizationId, int? groupId)
        {
            return this.accountTypeRepository.FindAll(organizationId, groupId, true);
        }
     }
}