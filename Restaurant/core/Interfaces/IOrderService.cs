using core.Entities;
using Microsoft.AspNetCore.Mvc;
using core.DTOs.OrderDto;

namespace core.Interfaces
{
    public interface IOrderService
    {
        Task<ActionResult> GetAllOrders();
        Task<ActionResult> GetOrderById(int id);
        Task<ActionResult> CreateOrder(OrderDto orderDto);
        Task<ActionResult> UpdateOrder(int id, OrderDto orderDto);
        Task<ActionResult> UpdateOrderStatus(UpdateStatus updateStatus);
        Task<ActionResult> DeleteOrder(int id);
    }
}
