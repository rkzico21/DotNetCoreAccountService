namespace AccountingService
{
    using System.Security.Claims;
    using AccountingService.Authentication;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Mvc;
    
    public static  class ControllerBaseExtension
    {
        public static int GetOrganizationId(this ControllerBase controller, UserService userService)
        {
            var organizationIdValue = AuthenticationHelper.GetClaim(controller.HttpContext, "Organization");
            int organizationId;
            
            if(int.TryParse(organizationIdValue, out organizationId))
            { 
                return organizationId;
            }
            else
            {
               var userEmail = AuthenticationHelper.GetClaim(controller.HttpContext, ClaimTypes.Name);
               var user =  userService.GetUserByEmail(userEmail);
               return ( user != null &&  user.OrganizationId.HasValue) ? user.OrganizationId.Value : -1;
            }
        }
    }
}