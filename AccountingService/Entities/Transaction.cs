namespace AccountingService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("transactions")]
    [JsonConverter(typeof(TransactionConverter))]
    public class Transaction : EntityBase
    {
        [Required]
        [Column("transaction_type")]
        public int? TransactionTypeId { get; set; }


        //[Required]
        [Column("account_id")]
        public  Guid? AccountId { get; set; }

        [Column("amount")]
        [Range(0.0, Double.MaxValue)]
        public double Amount { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }


        [JsonIgnore]
        [Column("organization_id")]
        public Guid OrganizationId { get; set; }

        [Column("note")]
        public string Note{get; set;}
        
        [JsonIgnore]
        public Account Account {get; set;}
    }
}