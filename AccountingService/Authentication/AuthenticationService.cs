namespace AccountingService.Authentication
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Collections.Generic;
    using AccountingService.Entities;
    using AccountingService.Exceptions;
    using AccountingService.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using AccountingService.Helpers;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;

    public class AuthenticationService
    {
        private readonly UserRepository userRepository;
        private readonly AppSettings appSettings;
        private readonly ILogger<AuthenticationService> logger;
        
        public AuthenticationService(UserRepository userRepository, IOptions<AppSettings> appSettings, ILogger<AuthenticationService> logger)
        {
            this.userRepository = userRepository;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public LoginResponse Authenticate(LoginModel loginModel)
        {
           var user = userRepository.FindUserByEmail(loginModel.Email);
           if(user == null)
           {
        
           }
           
           //TODO: match password

           var tokenHandler = new JwtSecurityTokenHandler();
           var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var response = new LoginResponse{
                Token = tokenHandler.WriteToken(token),
                User = new User{Id = user.Id, Email = user.Email}
            };

            return response;
        }
    }
}