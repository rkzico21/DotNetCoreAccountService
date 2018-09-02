namespace AccountingService.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Id {get; set;}
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required] 
        public int? AccountTypeId { get; set; }

        public int GroupId { get; set; }
    
    }
}