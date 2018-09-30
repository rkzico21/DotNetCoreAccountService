namespace AccountingService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Newtonsoft.Json;
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
              yield return new ValidationResult("Debit and Credit amounts are not equal");
          }
            
      }
    }

    public class TransactionItem : EntityBase
    {

        [Column("transaction_id")]
        public int TransactionId {get; set;}

        [Column("type")]
        [Required]
        public string TransactionType { get; set;}

        [Column("amount")]
        public Double  Amount {get; set;}

        [Column("account_id")]
        [Required]
        public int?  AccountId {get; set;}

        [JsonIgnore]
        [Column("organization_id")]
        public int? OrganizationId { get; set; }

        [JsonIgnore]
        public JournalTransaction Transaction {get; set;}

        [JsonIgnore]
        public Account Account {get; set;}

    }

    public abstract class JsonCreationConverter<T> : JsonConverter
   {
    /// <summary>
    /// this is very important, otherwise serialization breaks!
    /// </summary>
    public override bool CanWrite
    {
        get
        {
            return false;
        }
    }
    /// <summary> 
    /// Create an instance of objectType, based properties in the JSON object 
    /// </summary> 
    /// <param name="objectType">type of object expected</param> 
    /// <param name="jObject">contents of JSON object that will be 
    /// deserialized</param> 
    /// <returns></returns> 
    protected abstract T Create(Type objectType, JObject jObject);

    public override bool CanConvert(Type objectType)
    {
        return typeof(T).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType,
      object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;
        // Load JObject from stream 
        JObject jObject = JObject.Load(reader);

        // Create target object based on JObject 
        T target = Create(objectType, jObject);

        // Populate the object properties 
        serializer.Populate(jObject.CreateReader(), target);

        return target;
    }

    public override void WriteJson(JsonWriter writer, object value, 
      JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
} 


public class TransactionConverter : JsonCreationConverter<Transaction>
  {
     protected override Transaction Create(Type objectType, 
       Newtonsoft.Json.Linq.JObject jObject)
     {
       //TODO: read the raw JSON object through jObject to identify the type
       //e.g. here I'm reading a 'typename' property:

       if(jObject.Value<string>("transactionTypeId").Equals("3"))
       {
         return new JournalTransaction();
       }

       return new Transaction();

     }
  }
}