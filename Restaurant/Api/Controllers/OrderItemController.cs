using core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] 
    public class OrderItemController : Controller
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        // GET: api/OrderItem
        [HttpGet]
        public async Task<ActionResult> GetAllOrderItems()
        {
            return await _orderItemService.GetAllOrderItems();
        }

        // GET: api/OrderItem/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderItemById(int id)
        {
            return await _orderItemService.GetOrderItemById(id);
        }

        // GET: api/OrderItem/order/{orderId}
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult> GetOrderItemsByOrderId(int orderId)
        {
            return await _orderItemService.GetOrderItemsByOrderId(orderId);
        }

    }
}
