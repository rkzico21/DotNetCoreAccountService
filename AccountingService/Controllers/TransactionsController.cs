namespace AccountingService
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingService.Entities;
    using AccountingService.Filetes;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private TransactionService transactionService;
        private readonly ILogger<TransactionsController> logger;

        public TransactionsController(TransactionService transactionService, ILogger<TransactionsController> logger)
        {
            this.transactionService = transactionService;
            this.logger = logger;
        }
        
        [HttpGet]
        public IActionResult GetAccounts([FromQuery(Name="orgid")] int? organizationId)
        {
            return  Ok(transactionService.GetTransactions(organizationId));
        }


        [HttpGet("{id}", Name="GetTransaction")]
        public IActionResult GetTransaction(int id)
        {
            logger.LogDebug($"Get transaction with id : {id}");
            var transaction = transactionService.GetTransaction(id);
            return  Ok(transaction);
        }
        
        [HttpPost]
        [ValidateModel]
        public IActionResult  CreateAccount([FromBody] Transaction newTransaction)
        {
            var transaction =  transactionService.CreateTransaction(newTransaction);
            return CreatedAtRoute("GetTransaction", new { id = transaction.Id }, transaction);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            logger.LogDebug($"Deleting transaction with id : {id}");
            transactionService.Delete(id);
            return NoContent();
        }
     }
}