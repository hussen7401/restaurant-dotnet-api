
using System.ComponentModel.DataAnnotations;

namespace core.DTOs.OrderDto
{
    public class OrderItemDto
    {

        public int MenuItemId { get; set; }
        [Required, Range(1, 100)]
        public int Quantity { get; set; }
    }
    public class ShowOrderItem
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
