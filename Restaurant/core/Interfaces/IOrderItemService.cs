using core.DTOs.OrderDto;
using core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace core.Interfaces
{
    public interface IOrderItemService
    {
        Task<ActionResult> GetAllOrderItems();
        Task<ActionResult> GetOrderItemById(int id);
        Task<ActionResult> GetOrderItemsByOrderId(int orderId);
    }
}
