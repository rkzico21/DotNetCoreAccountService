namespace AccountingService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    [Table("transactions")]
    public class JournalTransaction : Transaction, IValidatableObject
    {
        public JournalTransaction()
        {
            this.TransactionTypeId = 3;
        }
        
      public IList<TransactionItem> Debits {get; set;}

      public IList<TransactionItem> Credits {get; set;}
    
      public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
      {
          if(this.Debits.Sum(d=>d.Amount) != this.Credits.Sum(c=>c.Amount))
          {
              yield return new ValidationResult("Debit and Credit amounts are not equal", new[] { "Debits", "Credits" });
          }
            
      }
    }
}