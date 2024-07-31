using core.DTOs.OrderDto;
using core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] 
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult> GetAllOrders()
        {
            return await _orderService.GetAllOrders();
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(int id)
        {
            return await _orderService.GetOrderById(id);
        }

        // POST: api/Order/create
        [HttpPost("create")]
        public async Task<ActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            return await _orderService.CreateOrder(orderDto);
        }

        // PUT: api/Order/update/{id}
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            return await _orderService.UpdateOrder(id, orderDto);
        }

        // PUT: api/Order/update-status
        [HttpPut("update-status")]
        public async Task<ActionResult> UpdateOrderStatus([FromBody] UpdateStatus updateStatus)
        {
            return await _orderService.UpdateOrderStatus(updateStatus);
        }

        // DELETE: api/Order/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            return await _orderService.DeleteOrder(id);
        }
    }
}
