namespace AccountingService.Authentication
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using AccountingService.Entities;

    public class LoginModel 
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }


    public class LoginResponse
    {
        public string Token { get; set; }
        
        public User User { get; set; }

        public int OrganizationId {get; set;}
    }
}