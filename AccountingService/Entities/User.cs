namespace AccountingService.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("users")]
    public class User : EntityBase
    {

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [JsonIgnore]
        [Required]
        [Column("password_hash")]
        public string Password { get; set; }

        [Column("organization_id")]
        public int? OrganizationId {get; set;}

        [JsonIgnore]
        public Organization Organization { get; set;}
    }
}