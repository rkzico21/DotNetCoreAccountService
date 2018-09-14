namespace AccountingService.Authentication
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    public static class AuthenticationHelper 
    {
        public static string GetClaim(Microsoft.AspNetCore.Http.HttpContext context, string type)
        {
             var claim = context.User.Claims.FirstOrDefault(c=>c.Type.Equals(type));
             return claim?.Value;
        }
    }

}