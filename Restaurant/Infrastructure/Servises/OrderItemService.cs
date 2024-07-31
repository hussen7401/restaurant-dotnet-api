using AutoMapper;
using core.Helpers;
using core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Servises
{
    public class OrderItemService : IOrderItemService
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly Responses _responses;

        public OrderItemService(MyDbContext context, IMapper mapper, Responses responses)
        {
            _context = context;
            _mapper = mapper;
            _responses = responses;
        }

        public async Task<ActionResult> GetAllOrderItems()
        {
            try
            {
                var orderItems = await _context.OrderItems.Include(oi => oi.Order).Include(oi => oi.MenuItem).ToListAsync();
                return _responses.ResponseSuccess("تم جلب بيانات عناصر الطلبات بنجاح .", orderItems);
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> GetOrderItemById(int id)
        {
            try
            {
                var orderItem = await _context.OrderItems.Include(oi => oi.Order).Include(oi => oi.MenuItem).FirstOrDefaultAsync(oi => oi.Id == id);
                if (orderItem == null)
                {
                    return _responses.ResponseNotFound("عنصر الطلب هذا غير متوفر ");
                }
                return _responses.ResponseSuccess("تم جلب بيانات عناصر الطلب بنجاح", orderItem);
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
        public async Task<ActionResult> GetOrderItemsByOrderId(int orderId)
        {
            try
            {
                var orderItems = await _context.OrderItems.Where(oi => oi.OrderId == orderId).Include(oi => oi.MenuItem).ToListAsync();
                if (orderItems == null )
                {
                    return _responses.ResponseNotFound("لا يوجد عناصر طلب لهذا الطلب!");
                }
                return _responses.ResponseSuccess("تم جلب بيانات عناصر الطلب بنجاح", orderItems);
            }
            catch (DbUpdateException ex)
            {
                return _responses.HandleException(ex);
            }
            catch (Exception ex)
            {
                return _responses.HandleException(ex);
            }
        }
    }
}
