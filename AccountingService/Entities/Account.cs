namespace AccountingService.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("accounts")]
    public class Account : EntityBase
    {
        
        
        [Required]
        [Column("name")]
        public string Name { get; set; }
        
        [Column("description")]
        public string Description { get; set; }

        [Required] 
        [Column("account_type_id")]
        public Guid AccountTypeId { get; set; }

        [Column("group_id")]
        public Guid GroupId { get; set; }

        [Column("organization_id")]
        public Guid OrganizationId { get; set;}
    
    }

    public class EntityBase 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        [Column("id")]
        public Guid Id {get; set;}
    }
}