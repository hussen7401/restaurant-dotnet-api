using core.DTOs.AccountDto;
using core.Entities;
using core.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.DTOs.OrderDto
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
    }
    public class ShowOrder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreateAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
        public UserInfoDto? UserInfo { get; set; }
        public ICollection<ShowOrderItem> ShowOrderItem { get; set; }
    }
}
