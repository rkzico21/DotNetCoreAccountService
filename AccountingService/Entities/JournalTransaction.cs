namespace AccountingService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("transactions")]
    public class JournalTransaction : Transaction
    {
        public JournalTransaction()
        {
            this.TransactionTypeId = 3;
        }
        
      public IList<TransactionItem> Items {get; set;}
    
    }


    [Table("transaction_items")]
    public class TransactionItem : EntityBase
    {

        [Column("transaction_id")]
        public int TransactionId {get; set;}

        [Column("type")]
        public string TransactionType { get; set;}

        [Column("amount")]
        public Double  Amount {get; set;}

        [JsonIgnore]
        public JournalTransaction Transaction {get; set;}

    }
}