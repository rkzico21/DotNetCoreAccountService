namespace AccountingService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    //TODO: make it enum
    public class TransactionType
    {
        public int Id {get; set;}
        
        public string Name {get; set;}
    }
}