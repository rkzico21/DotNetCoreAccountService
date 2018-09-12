namespace AccountingService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("transactions")]
    public class Transaction : EntityBase
    {
        [Required]
        [Column("transaction_type")]
        public int? TransactionTypeId { get; set; }


        [Required]
        [Column("account_id")]
        public int? AccountId { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        //TODO: add other category. or categories
    }
}