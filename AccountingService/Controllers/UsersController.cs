namespace AccoutingService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingService.Entities;
    using AccountingService.Filetes;
    using AccountingService.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private readonly UserService UserService; 
        private readonly ILogger<UsersController> logger;

        public UsersController(UserService UserService, ILogger<UsersController> logger)
        {
            this.UserService = UserService;
            this.logger = logger;
        }


        // GET api/Users
        [HttpGet]
        public IActionResult GetUsers()
        {
             return Ok(this.UserService.GetUsers());
        }

        // GET api/Users/5
        [HttpGet("{id}", Name="GetUser")]
        public IActionResult GetUser(int id)
        {
            return Ok(this.UserService.GetUser(id));
        }

        // POST api/Users
        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody] User neweUser)
        {
           var User = this.UserService.CreateUser(neweUser);
           return CreatedAtRoute("GetUser", new { id = User.Id }, User);
        }


        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] User neweUser)
        {
           var User = this.UserService.CreateUser(neweUser);
           return CreatedAtRoute("GetUser", new { id = User.Id }, User);
        }

        // PUT api/Users/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, User User)
        {
            return Ok(this.UserService.UpdateUser(id, User));
        }

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.UserService.Delete(id);
            return NoContent();
        }
    }
}
