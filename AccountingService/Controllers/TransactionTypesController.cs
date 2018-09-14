namespace AccountingService
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingService.Entities;
    using AccountingService.Filetes;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionTypesController : ControllerBase
    {
        private TransactionService transactionService;
        private readonly ILogger<TransactionsController> logger;

        public TransactionTypesController(TransactionService transactionService, ILogger<TransactionsController> logger)
        {
            this.transactionService = transactionService;
            this.logger = logger;
        }
        
        [HttpGet]
        public IActionResult GetTransactionTypes()
        {
            return  Ok(transactionService.GetTransactionTypes());
        }
    }
}