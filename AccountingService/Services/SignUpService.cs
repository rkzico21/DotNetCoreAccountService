namespace AccountingService.Services
{
    using System;
    using System.Collections.Generic;
    using AccountingService.Entities;
    using AccountingService.Exceptions;
    using AccountingService.Helpers;
    using AccountingService.Models;
    using AccountingService.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class SignUpService
    {
        private readonly UserService userService;
        private readonly PasswordHasher<SignUpModel> passwordHasher;

        private readonly ILogger<SignUpService> logger;

        public SignUpService(UserService  userService, PasswordHasher<SignUpModel> passwordHasher,  ILogger<SignUpService> logger)
        {
            this.userService = userService;
            this.passwordHasher = passwordHasher;
            this.logger = logger;
        } 
        
        public User SignUp(SignUpModel model)
        {
            //TODO: check duplication
           if( this.userService.UserExists(model.Email))
           {
               throw new Exception("User alredy exist");
           }
            
            //TODO: fix name parameter
            var user = new User { Name= model.Email, Email= model.Email, Password= this.passwordHasher.HashPassword(model, model.Password)};
            return userService.CreateUser(user);
        }
    }
}