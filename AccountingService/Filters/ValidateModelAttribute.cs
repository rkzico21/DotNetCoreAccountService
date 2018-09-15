
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountingService.DbContexts;
using AccountingService.Entities;
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


    public class TransactionAccessRequirement: IAuthorizationRequirement
    {

    }

    public class AccountAccessRequirement :  IAuthorizationRequirement
    {

    }


    public class TransactionAccessHandler : AuthorizationHandler<TransactionAccessRequirement>
    {
        private readonly AccountingDbContext dbContext;
        
        public TransactionAccessHandler(AccountingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TransactionAccessRequirement requirement)
        {
            if(context.Resource is AuthorizationFilterContext mvcContext)
            {
                var request = mvcContext.HttpContext.Request;
                int? organizationId = null;
                if(request.Method == HttpMethods.Get || request.Method == HttpMethods.Delete || request.Method == HttpMethods.Put)
                {
                    var routeDate = mvcContext.RouteData;
                    if(routeDate.Values.ContainsKey("id"))
                    {
                        int id;
                        int.TryParse(routeDate.Values["id"].ToString(), out id); 
                        var transaction = this.dbContext.Transactions.FirstOrDefault(a=>a.Id == id);
                        organizationId = transaction?.OrganizationId;
                    }
                }
                
                if(request.Method == HttpMethods.Post || request.Method == HttpMethods.Put)
                {
                    request.EnableBuffering();
                    try
                    {
                        using(var reader = new StreamReader(request.Body, Encoding.UTF8))
                        {
                            var response = reader.ReadToEnd();
                            var transaction = JsonConvert.DeserializeObject(response, typeof(Transaction)) as Transaction;
                            if(transaction != null)
                            {
                                organizationId = dbContext.Accounts.FirstOrDefault(a=>a.Id == transaction.AccountId)?.OrganizationId;
                                if(organizationId.HasValue && IsUserOrganizationDiffrent(context, organizationId.Value))
                                {
                                    return Task.CompletedTask;
                                }
                            }

                            request.Body.Position = 0;
                        }
                    }
                    catch(Exception ex)
                    {
                            //TODO: log error
                    }
                }

                if(organizationId.HasValue && IsUserOrganizationDiffrent(context, organizationId.Value))
                {
                    return Task.CompletedTask;
                }
            }
            
            context.Succeed(requirement); 
            return Task.CompletedTask;
        }

        private bool IsUserOrganizationDiffrent(AuthorizationHandlerContext context, int organizationId)
        {
            if(!context.User.HasClaim(c => c.Type == "Organization" && c.Value == organizationId.ToString()))
            {
                context.Fail();
                return true;
            }

            return false;
        }

        
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
                        
                        if(account != null && !context.User.HasClaim(c=>c.Type == "Organization" && c.Value == account.OrganizationId.ToString()))
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