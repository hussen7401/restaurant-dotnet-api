using core.enums;
using System.ComponentModel.DataAnnotations;

namespace core.DTOs.OrderDto
{
    public class UpdateStatus
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Range(0, 2)]
        public OrderStatus Status { get; set; }
    }
}
