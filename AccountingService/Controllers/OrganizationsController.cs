namespace AccoutingService.Controllers
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
        public IActionResult GetOrganization(int id)
        {
            return Ok(this.organizationService.GetOrganization(id));
        }

        // POST api/organizations
        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody] Organization neweOrganization)
        {
           var organization = this.organizationService.CreateOrganization(neweOrganization);
           return CreatedAtRoute("GetOrganization", new { id = organization.Id }, organization);
        }

        // PUT api/organizations/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Organization organization)
        {
            return Ok(this.organizationService.UpdateOrganization(id, organization));
        }

        // DELETE api/organizations/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.organizationService.Delete(id);
            return NoContent();
        }
    }
}
