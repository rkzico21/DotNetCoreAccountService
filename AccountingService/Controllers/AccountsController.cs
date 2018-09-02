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
    [ValidateModel]
    public class AccountsController : ControllerBase
    {
        private AccountService accountService;
        private readonly ILogger<AccountsController> logger;

        public AccountsController(AccountService accountService, ILogger<AccountsController> logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }
        
        [HttpGet]
        public IActionResult GetAccounts([FromQuery(Name="orgid")] int? organizationId, [FromQuery(Name="group")] int? group, [FromQuery(Name="type")] int? accountType)
        {
            return  Ok(accountService.GetAccounts(organizationId, group, accountType));
        }


        [HttpGet("{id}", Name="GetAccount")]
        public IActionResult GetAccount(int id)
        {
            logger.LogDebug($"Get account with id : {id}");
            var account = accountService.GetAccount(id);
            return  Ok(account);
        }
        
        [HttpPost]
        public IActionResult  CreateAccount([FromBody] Account newAccount)
        {
            var account =  accountService.CreateAccount(newAccount);
            return CreatedAtRoute("GetAccount", new { id = account.Id }, account);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            logger.LogDebug($"Deleting account with id : {id}");
            accountService.Delete(id);
            return NoContent();
        }
     }
}