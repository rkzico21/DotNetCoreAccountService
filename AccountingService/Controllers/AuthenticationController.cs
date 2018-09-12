using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AccoutingService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class Autehntication : ControllerBase
    {
        [HttpPost("{login}")]
        public IActionResult Authenticate([FromBody] LoginModel loginModel )
        {
            return Ok(new LoginResponse{Token= "123213", User = new User {Id=1, Name="name"} }); 
        }
            
        
    }
}
