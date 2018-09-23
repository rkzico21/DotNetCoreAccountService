namespace AccountingService.Services
{
    using System;
    using System.Collections.Generic;
    using AccountingService.Entities;
    using AccountingService.Exceptions;
    using AccountingService.Repositories;
    using Microsoft.Extensions.Logging;
    
    public class UserService
    {
        private readonly UserRepository repository;
        private readonly ILogger<UserService> logger;

        public UserService(UserRepository  repository,ILogger<UserService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public IEnumerable<User> GetUsers()
        {
             return this.repository.FindAll();
        }

        public User GetUser(int id)
        {
             User User = this.repository.FindById(id);
             if(User == null)
             {
                var message = $"User with id: {id} not found";
                logger.LogWarning(message);
                throw new ResourceNotFoundException(message); 
             }

             return User;
        }

        public User CreateUser(User User)
        {
            return repository.Add(User);
        }

        public User UpdateUser(int id, User User)
        {
            throw new NotImplementedException();
        }
        
        public void Delete(int id)
        {
            this.repository.Delete(id);
        }
    }
}