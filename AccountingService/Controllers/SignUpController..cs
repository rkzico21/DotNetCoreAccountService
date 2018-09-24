namespace AccoutingService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingService.Entities;
    using AccountingService.Filetes;
    using AccountingService.Models;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/signup")]
    [ApiController]
    [AllowAnonymous]
    public class SignUpController : ControllerBase
    {

        private readonly SignUpService service; 
        private readonly ILogger<SignUpController> logger;

        public SignUpController(SignUpService service, ILogger<SignUpController> logger)
        {
            this.service = service;
            this.logger = logger;
        }
        
        [HttpPost]
        [ValidateModel]
        public IActionResult SignUp([FromBody] SignUpModel signUpModel)
        {
           var user = this.service.SignUp(signUpModel);
           return Ok();
        }
    }
}
