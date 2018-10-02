namespace AccountingService.Entities
{
    using System;

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