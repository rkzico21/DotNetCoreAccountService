namespace AccountingService.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("organizations")]
    public class Organization : EntityBase
    {

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Account> Accounts { get; set;}
    }
}