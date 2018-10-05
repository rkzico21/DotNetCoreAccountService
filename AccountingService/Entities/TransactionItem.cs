namespace AccountingService.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("transaction_items")]
    public class TransactionItem : EntityBase
    {

        [Column("transaction_id")]
        public Guid TransactionId { get; set;}

        [Column("type")]
        [Required]
        public string TransactionType { get; set;}

        [Column("amount")]
        [Range(0.0, Double.MaxValue)]
        public Double  Amount {get; set;}

        [Column("account_id")]
        [Required]
        public Guid AccountId {get; set;}

        [JsonIgnore]
        [Column("organization_id")]
        public Guid OrganizationId { get; set; }

        [JsonIgnore]
        public JournalTransaction Transaction {get; set;}

        [JsonIgnore]
        public Account Account {get; set;}

    }


    public class Debit: TransactionItem
    {
        public Debit()
        {
            this.TransactionType = "debit";
        }
    }


    public class Crebit: TransactionItem
    {
        public Crebit()
        {
            this.TransactionType = "credit";
        }
    }
}