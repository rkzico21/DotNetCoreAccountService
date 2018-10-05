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
        private readonly OrganizationService organizationService;
        private readonly ILogger logger;
        
        public AccountService(AccountRepository repository, AccountGroupRepository accountGroupRepository, AccountTypeRepository accountTypeRepository, OrganizationService organizationService, ILogger<AccountService> logger)
        {
            this.repository = repository;
            this.accountGroupRepository = accountGroupRepository;
            this.accountTypeRepository = accountTypeRepository;
            this.organizationService = organizationService;
            this.logger = logger;
        }

        public IEnumerable<Account> GetAccounts(string organizationId, string group, string accountType)
        {
            return  repository.FindAll(organizationId, group, accountType);
        }

        public Account CreateAccount(Account account, string organizationId)
        {
            var accountType = this.accountTypeRepository.FindById(account.AccountTypeId.ToString());
            if(accountType == null)
            {
                throw new ResourceNotFoundException($"Account Type with id:{account.AccountTypeId.ToString()} does not exist"); 
            }
            

            account.GroupId = accountType.GroupId; //make sure correct group id has been assigned.
            account.OrganizationId = this.organizationService.GetOrganization(organizationId).Id;
            return this.repository.Add(account);
        }
        
        public void Delete(string id)
        {
            this.repository.Delete(id);
        }

        public Account GetAccount(string id)
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

        public IEnumerable<AccountType> GetAccountTypes(string organizationId, string groupId)
        {
            return this.accountTypeRepository.FindAll(groupId, true , organizationId);
        }
     }
}