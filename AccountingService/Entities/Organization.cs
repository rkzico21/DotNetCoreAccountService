namespace AccountingService.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("organizations")]
    public class Organization : EntityBase
    {

        [Required]
        [Column("name")]
        public string Name { get; set; }
    }
}