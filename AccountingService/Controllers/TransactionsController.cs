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
        private readonly ILogger<TransactionsController> logger;

        public TransactionsController(TransactionService transactionService, ILogger<TransactionsController> logger)
        {
            this.transactionService = transactionService;
            this.logger = logger;
        }
        
        [HttpGet]
        public IActionResult GetTransactions()
        {
            var organizationId = GetOrganizationId();
            return  organizationId <=0 ?  Ok(Enumerable.Empty<Transaction>()) : Ok(transactionService.GetTransactions(organizationId));
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
        public IActionResult  CreateTransaction([FromBody] Transaction newTransaction)
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

        private int GetOrganizationId()
        {
            var organizationIdValue = AuthenticationHelper.GetClaim(this.HttpContext, "Organization");
            int organizationId;
            return  int.TryParse(organizationIdValue, out organizationId) ? organizationId : -1;
        }
     }
}