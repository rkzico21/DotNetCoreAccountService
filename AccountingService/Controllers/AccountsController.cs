namespace AccountingService
{

    using System;
    using System.Collections.Generic;
    using System.IdentityModel;
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
    [Authorize]
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
        public IActionResult GetAccounts([FromQuery(Name="group")] int? group, [FromQuery(Name="type")] int? accountType)
        {
            var organizationId = this.GetOrganizationId();
            return organizationId >=1 ? Ok(accountService.GetAccounts(organizationId, group, accountType)) 
                                : Ok(Enumerable.Empty<Account>());   
            
        }


        [HttpGet("{id}", Name="GetAccount")]
        public IActionResult GetAccount(int id)
        {
            logger.LogDebug($"Get account with id : {id}");
            var account = accountService.GetAccount(id);
            return  Ok(account);
        }
        
        [HttpPost]
        [ValidateModel]
        public IActionResult  CreateAccount([FromBody] Account newAccount)
        {
            var organizationId = this.GetOrganizationId();
            var account =  accountService.CreateAccount(newAccount, organizationId);
            return CreatedAtRoute("GetAccount", new { id = account.Id }, account);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            logger.LogDebug($"Deleting account with id : {id}");
            accountService.Delete(id);
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