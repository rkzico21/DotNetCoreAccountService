namespace AccountingService.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AccountType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int GroupId { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}