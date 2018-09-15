

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingService.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace AccountingService.Filetes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState) 
            : base(new ValidationResultModel(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
        

    public class ValidationError
    {
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Field { get; }
            
        public string Message { get; }
        
        public ValidationError(string field, string message)
        {
            this.Field = field != string.Empty ? field : null;
            this.Message = message;
        }
    }
    
    public class ValidationResultModel : ApiErrorMessage
    {
        public List<ValidationError> Errors { get; }
        
        public ValidationResultModel(ModelStateDictionary modelState)
            : base("Validation Failed")
        {
            Errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();
        }
        
    }

    public class ApiErrorMessage 
    {   
        public string Message { get;  } 

        public ApiErrorMessage(string message)
        {
            this.Message = message;
        }
        
    }

    public class AccountAccessRequirement :  IAuthorizationRequirement
    {

    }

    public class AccountAccessHandler : AuthorizationHandler<AccountAccessRequirement>
    {
        private readonly AccountingDbContext dbContext;

        public AccountAccessHandler(AccountingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccountAccessRequirement requirement)
        {
            //Get organizationn id 
            if(context.Resource is AuthorizationFilterContext mvcContext)
            {
                if(mvcContext.HttpContext.Request.Method == HttpMethods.Get || mvcContext.HttpContext.Request.Method == HttpMethods.Delete)
                {
                    var routeDate = mvcContext.RouteData;
                    if(routeDate.Values.ContainsKey("id"))
                    {
                        int id;
                        int.TryParse(routeDate.Values["id"].ToString(), out id); 
                        var account = this.dbContext.Accounts.FirstOrDefault(a=>a.Id == id);
                        
                        if(account != null && !context.User.HasClaim(c=>c.Type == "Organization" && c.Value == account.Id.ToString()))
                        {
                            context.Fail();
                            return Task.CompletedTask;
                        }   
                    }
                }
             }

            context.Succeed(requirement); 
            return Task.CompletedTask;
        }
    }
}