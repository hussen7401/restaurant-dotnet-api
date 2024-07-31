using System.ComponentModel.DataAnnotations;

namespace core.Entities
{
    public class MenuItem : BaseEntity
    {
        [Required, StringLength(30)]
        public string Name { get; set; }
        [Required, StringLength(200)]
        public string Description { get; set; }
        [Required, Range(0.01, 1000)]
        public decimal Price { get; set; }
        [Required]
        public bool IsAvailable { get; set; } 
    }
}
