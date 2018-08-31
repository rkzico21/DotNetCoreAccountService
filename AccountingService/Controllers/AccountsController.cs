namespace AccountingService
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingService.Entities;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private AccountService accountService;
        
        public AccountsController(AccountService accountService)
        {
            this.accountService = accountService;
        }
        
        [HttpGet]
        public IActionResult GetAccounts([FromQuery(Name="orgid")] int? organizationId, [FromQuery(Name="group")] int? group, [FromQuery(Name="type")] int? accountType)
        {
            if(!accountType.HasValue && !group.HasValue && !organizationId.HasValue) 
            {
                return  Ok(accountService.GetAccounts());
            }   
            else
            {
                return  Ok(accountService.GetAccounts(organizationId, group, accountType));
            }
        }


        [HttpGet("{id}", Name="GetAccount")]
        public IActionResult GetAccount(int id)
        {
            var account = accountService.GetAccount(id);
            if (account == null)
            {
                return NotFound();
            }
            
            return  Ok(account);
        }


        
        [HttpPost]
        public IActionResult  CreateAccount([FromBody] Account account)
        {
            if(ModelState.IsValid)
            {
                accountService.CreateAccount(account);
                return CreatedAtRoute("GetAccount", new { id = account.Id }, account);
            }
            
            return ValidationProblem();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            accountService.Delete(id);
            return NoContent();
        }
     }
}