namespace AccountingService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("transactions")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        [Column("id")]
        public int Id {get; set;}
        
        [Required]
        [Column("account_id")]
        public int? AccountId { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        [DataType(DataType.Date)]
        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        //TODO: add other category. or categories
    }
}