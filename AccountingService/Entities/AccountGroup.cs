namespace AccountingService.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("account_groups")]
    public class AccountGroup : EntityBase
    {
        [Required]
        [Column("name")]
        public string Name { get; set; }
     
    }
}