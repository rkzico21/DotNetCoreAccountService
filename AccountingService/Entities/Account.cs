namespace AccountingService.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("accounts")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        [Column("id")]
        public int Id {get; set;}
        
        [Required]
        [Column("name")]
        public string Name { get; set; }
        
        [Column("description")]
        public string Description { get; set; }

        [Required] 
        [Column("account_type_id")]
        public int? AccountTypeId { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }
    
    }
}