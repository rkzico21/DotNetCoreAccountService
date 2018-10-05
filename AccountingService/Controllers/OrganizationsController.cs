namespace AccoutingService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
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
    public class OrganizationsController : ControllerBase
    {

        private readonly OrganizationService organizationService; 
        private readonly ILogger<OrganizationsController> logger;

        public OrganizationsController(OrganizationService organizationService, ILogger<OrganizationsController> logger)
        {
            this.organizationService = organizationService;
            this.logger = logger;
        }


        // GET api/organizations
        [HttpGet]
        public IActionResult GetOrganizations()
        {
             return Ok(this.organizationService.GetOrganizations());
        }

        // GET api/organizations/5
        [HttpGet("{id}", Name="GetOrganization")]
        public IActionResult GetOrganization(string id)
        {
            return Ok(this.organizationService.GetOrganization(id));
        }

        // POST api/organizations
        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody] Organization neweOrganization)
        {
           var userEmail = AccountingService.Authentication.AuthenticationHelper.GetClaim(this.HttpContext, ClaimTypes.Name);
           var organization = this.organizationService.CreateOrganization(neweOrganization, userEmail);
           return CreatedAtRoute("GetOrganization", new { id = organization.Id }, organization);
        }
    }
}
