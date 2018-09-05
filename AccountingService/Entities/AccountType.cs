namespace AccountingService.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("account_types")]
    public class AccountType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        [Column("id")]
        public int Id {get; set;}
        
        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}