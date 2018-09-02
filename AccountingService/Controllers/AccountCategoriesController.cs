namespace AccountingService
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingService.Entities;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/accountcategory")]
    [ApiController]
    public class AccountCategoriesController : ControllerBase
    {
        private AccountService accountService;
        
        public AccountCategoriesController(AccountService accountService)
        {
            this.accountService = accountService;
        }
        
        [HttpGet]
        public IActionResult GetGroups([FromQuery(Name="orgid")] int? organizationId)
        {
            return  Ok(accountService.GetGroups());
        }


        [HttpGet("types")]
        public IActionResult GetAccountType([FromQuery(Name="orgid")] int? organizationId,  [FromQuery(Name="group")]int? groupId)
        {
            return  Ok(accountService.GetAccountTypes(organizationId, groupId));
        }
    }
}