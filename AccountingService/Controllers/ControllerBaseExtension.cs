namespace AccountingService
{
    using System.Security.Claims;
    using AccountingService.Authentication;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Mvc;
    
    public static  class ControllerBaseExtension
    {
        public static string GetOrganizationId(this ControllerBase controller, UserService userService)
        {
            var organizationIdValue = AuthenticationHelper.GetClaim(controller.HttpContext, "Organization");
            
            if(!string.IsNullOrWhiteSpace(organizationIdValue))
            { 
                return organizationIdValue;
            }
            else
            {
               var userEmail = AuthenticationHelper.GetClaim(controller.HttpContext, ClaimTypes.Name);
               var user =  userService.GetUserByEmail(userEmail);
               return ( user != null &&  user.OrganizationId != null) ? user.OrganizationId.ToString() : null;
            }
        }
    }
}