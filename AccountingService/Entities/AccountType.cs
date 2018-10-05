namespace AccountingService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("account_types")]
    public class AccountType : EntityBase
    {

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("group_id")]
        public Guid GroupId { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}