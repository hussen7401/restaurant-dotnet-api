using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using core.enums;

namespace core.Entities
{
    public class Order : BaseEntity
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required, Range(0.01, 10000)]
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.New;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }


}
