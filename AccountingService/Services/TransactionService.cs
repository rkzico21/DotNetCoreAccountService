namespace AccountingService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public IEnumerable<Transaction> GetTransactions(string organizationId) => repository.FindAll(organizationId);

        public Transaction GetTransaction(string id)
        {
            var transaction = repository.FindById(id);
            
            if(transaction == null)
            {
                var message = $"Transaction with id: {id} not found";
                logger.LogWarning(message);
                throw new ResourceNotFoundException(message); 
            }

            if(transaction is JournalTransaction journalTransaction)
            {
                this.repository.LoadItems(journalTransaction);
                return journalTransaction;
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
            if(transaction is JournalTransaction journalTransaction)
            {
               var accountId =  journalTransaction.Credits.FirstOrDefault()?.AccountId;
               journalTransaction.Amount = journalTransaction.Credits.Sum(c=>c.Amount);
            }
            else
            {
                var account = this.accountRepository.FindById(transaction.AccountId.ToString());
                if(account == null)
                {
                    var message = $"Account with id {transaction.AccountId.ToString()} not found";
                    logger.LogWarning(message);
                    throw new ResourceNotFoundException(message); 
                }

                transaction.OrganizationId = account.OrganizationId; 
            }
            
            if(!transaction.TransactionDate.HasValue)
            {
                this.logger.LogInformation("No transaction date has been mentioned. Assigning Current date.");
                transaction.TransactionDate = DateTime.Now;
            }

            return repository.Add(transaction);
        }



        public void Delete(string id) => repository.Delete(id);
    }
}