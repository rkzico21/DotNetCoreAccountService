namespace AccountingService
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingService.Authentication;
    using AccountingService.Entities;
    using AccountingService.Filetes;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy="TransactionAccessPolicy")]
    public class TransactionsController : ControllerBase
    {
        private TransactionService transactionService;
        private readonly UserService userService;
        private readonly ILogger<TransactionsController> logger;

        public TransactionsController(TransactionService transactionService, UserService userService, ILogger<TransactionsController> logger)
        {
            this.transactionService = transactionService;
            this.userService = userService;
            this.logger = logger;
        }
        
        [HttpGet]
        public IActionResult GetTransactions()
        {
            var organizationId = this.GetOrganizationId(this.userService);
            return  string.IsNullOrWhiteSpace(organizationId) ?  Ok(Enumerable.Empty<Transaction>()) : Ok(transactionService.GetTransactions(organizationId));
        }


        [HttpGet("{id}", Name="GetTransaction")]
        public IActionResult GetTransaction(string id)
        {
            logger.LogDebug($"Get transaction with id : {id}");
            var transaction = transactionService.GetTransaction(id);
            return  Ok(transaction);
        }
        
        [HttpPost]
        [ValidateModel]
        public IActionResult  CreateTransaction([FromBody] Transaction newTransaction)
        {
            if(TryValidateModel(newTransaction))
            {
                newTransaction.OrganizationId = Guid.Parse(this.GetOrganizationId(this.userService)); 
                var transaction =  transactionService.CreateTransaction(newTransaction);
                return CreatedAtRoute("GetTransaction", new { id = transaction.Id }, transaction);
            }

            return new ValidationFailedResult(ModelState);
        
            
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(string id)
        {
            logger.LogDebug($"Deleting transaction with id : {id}");
            transactionService.Delete(id);
            return NoContent();
        }
    }
}