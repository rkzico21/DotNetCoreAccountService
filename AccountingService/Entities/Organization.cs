using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingService.Entities
{
    public class Organization : EntityBase
    {
        
        [Required]
        [Column("name")]
        public string Name { get; set; }
    }
}