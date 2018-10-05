namespace AccountingService.Services
{
    using System;
    using System.Collections.Generic;
    using AccountingService.Entities;
    using AccountingService.Exceptions;
    using AccountingService.Repositories;
    using Microsoft.Extensions.Logging;
    
    public class OrganizationService
    {
        private readonly OrganizationRepository repository;
        private readonly ILogger<OrganizationService> logger;

        public OrganizationService(OrganizationRepository  repository,ILogger<OrganizationService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public IEnumerable<Organization> GetOrganizations() => this.repository.FindAll();

        public Organization GetOrganization(string id)
        {
             Organization organization = this.repository.FindById(id);
             if(organization == null)
             {
                var message = $"Organization with id: {id} not found";
                logger.LogWarning(message);
                throw new ResourceNotFoundException(message); 
             }

             return organization; 
        }

        public Organization CreateOrganization(Organization organization, string ownerEmail) 
        {
             return  repository.Add(organization, ownerEmail); 
        }
    }
}