using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingService.Authentication;
using AccountingService.Entities;
using AccountingService.Filetes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccoutingService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AutehnticationController : ControllerBase
    {
        private readonly AuthenticationService authenticationService;
        private readonly ILogger<AutehnticationController> logger;

        public AutehnticationController(AuthenticationService authenticationService, ILogger<AutehnticationController> logger) 
        {
            this.authenticationService = authenticationService;
            this.logger = logger;
        }

        [HttpPost("{login}")]
        [ValidateModel]
        public IActionResult Authenticate([FromBody] LoginModel loginModel )
        {
            return Ok(authenticationService.Authenticate(loginModel));
        }
    }
}
