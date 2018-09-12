namespace AccountingService.Services
{
    using System;
    using System.Collections.Generic;
    using AccountingService.Entities;
    using AccountingService.Exceptions;
    using AccountingService.Repositories;
    using Microsoft.Extensions.Logging;

    public class TransactionService
    {
        private readonly TransactionRepository repository;
        private readonly AccountRepository accountRepository;
        private readonly ILogger<TransactionService> logger;
        
        public TransactionService(TransactionRepository repository, AccountRepository accountRepository, ILogger<TransactionService> logger)
        {
            this.repository = repository;
            this.accountRepository = accountRepository;
            this.logger = logger;
        }

        public IEnumerable<Transaction> GetTransactions(int? organizationId) => repository.FindAll(organizationId);

        public Transaction GetTransaction(int id)
        {
            var transaction = repository.FindById(id);
             if(transaction == null)
            {
                var message = $"Transaction with id: {id} not found";
                logger.LogWarning(message);
                throw new ResourceNotFoundException(message); 
            }

            return transaction;
        }

        public IEnumerable<TransactionType> GetTransactionTypes() => new List<TransactionType>
            {
                new TransactionType {Id=1, Name="Income"},
                new TransactionType {Id=2, Name="Expense"},
                new TransactionType {Id=3, Name="Journal"}
            };

        public Transaction CreateTransaction(Transaction transaction)
        {
            var account = this.accountRepository.FindById(transaction.AccountId.Value);
            if(account == null)
            {
                var message = $"Account with id {transaction.AccountId.Value} not found";
                logger.LogWarning(message);
                throw new ResourceNotFoundException(message); 
            }
            
            if(!transaction.TransactionDate.HasValue)
            {
                this.logger.LogInformation("No transaction date has been mentioned. Assigning Current date.");
                transaction.TransactionDate = DateTime.Now;
            }

            return repository.Add(transaction);
        }

        public void Delete(int id) => repository.Delete(id);
    }
}