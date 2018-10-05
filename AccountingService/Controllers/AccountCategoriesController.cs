namespace AccountingService
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingService.Entities;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/accountcategory")]
    [ApiController]
    [Authorize]
    public class AccountCategoriesController : ControllerBase
    {
        private AccountService accountService;
        
        public AccountCategoriesController(AccountService accountService)
        {
            this.accountService = accountService;
        }
        
        [HttpGet]
        public IActionResult GetGroups([FromQuery(Name="orgid")] string organizationId)
        {
            return  Ok(accountService.GetGroups());
        }


        [HttpGet("types")]
        public IActionResult GetAccountType([FromQuery(Name="orgid")] string organizationId,  [FromQuery(Name="group")]string groupId)
        {
            return  Ok(accountService.GetAccountTypes(organizationId, groupId));
        }
    }
}