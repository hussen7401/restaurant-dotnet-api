using AutoMapper;
using core.Entities;
using core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using core.DTOs.OrderDto;
using core.Helpers;
using core.enums;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly Responses _responses;

        public OrderService(MyDbContext context, IMapper mapper, Responses responses)
        {
            _context = context;
            _mapper = mapper;
            _responses = responses;
        }
        public async Task<ActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _context.Orders.Include(o => o.OrderItems).Include(o => o.User).ToListAsync();
                if (orders == null)
                {
                    return _responses.ResponseNotFound("لا توجد طلبات للعرض!");
                }

                var showOrderList = _mapper.Map<List<ShowOrder>>(orders);

                return _responses.ResponseSuccess("تم جلب الطلبات بنجاح.", showOrderList);
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
        public async Task<ActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
                if (order == null)
                {
                    return _responses.ResponseNotFound("الطلب غير موجود!");
                }
                var showOrder = _mapper.Map<ShowOrder>(order);

                return _responses.ResponseSuccess("تم جلب بيانات الطلب بنجاح.", showOrder);
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
        public async Task<ActionResult> CreateOrder(OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return _responses.ResponseBadRequest("بيانات الطلب غير صحيحة.");
            }
            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == orderDto.UserId);
                if (!userExists)
                {
                    return _responses.ResponseBadRequest("المستخدم غير موجود.");
                }

                var order = _mapper.Map<Order>(orderDto);
                order.OrderItems = new List<OrderItem>();
                order.TotalAmount = 0;

                foreach (var itemDto in orderDto.OrderItems)
                {
                    var menuItem = await _context.MenuItems.FindAsync(itemDto.MenuItemId);
                    if (menuItem == null)
                    {
                        return _responses.ResponseBadRequest("لم يتم العثور على الوجبة");
                    }

                    var orderItem = _mapper.Map<OrderItem>(itemDto);
                    orderItem.UnitPrice = menuItem.Price;

                    order.OrderItems.Add(orderItem);
                    order.TotalAmount += orderItem.UnitPrice * orderItem.Quantity;
                }

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return _responses.ResponseSuccess("تم إنشاء الطلب بنجاح.", order);
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
        public async Task<ActionResult> UpdateOrder(int orderid, OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return _responses.ResponseBadRequest("بيانات الطلب غير صحيحة.");
            }
            try
            {
                var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderid);
                if (order == null)
                {
                    return _responses.ResponseNotFound("الطلب غير موجود.");
                }
                if (order.UserId != orderDto.UserId)
                {
                    return _responses.ResponseBadRequest("لا يمكنك تعديل هذا الطلب");
                }
                if (order.Status ==OrderStatus.InProgress || order.Status == OrderStatus.Completed)
                {
                    return _responses.ResponseBadRequest("لا يمكنك تعديل هذا الطلب بعد الان ");
                }
                _context.OrderItems.RemoveRange(order.OrderItems);
                _mapper.Map(orderDto, order);
                order.TotalAmount = 0;
                

                foreach (var itemDto in orderDto.OrderItems)
                {
                    var menuItem = await _context.MenuItems.FindAsync(itemDto.MenuItemId);
                    if (menuItem == null)
                    {
                        return _responses.ResponseBadRequest("لم يتم العثور على الوجبة");
                    }
                    var orderItem = _mapper.Map<OrderItem>(itemDto);
                    orderItem.UnitPrice = menuItem.Price;

                    order.OrderItems.Add(orderItem);
                    order.TotalAmount += orderItem.UnitPrice * orderItem.Quantity;
                }

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess("تم تحديث الطلب بنجاح.", order);
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
        public async Task<ActionResult> UpdateOrderStatus(UpdateStatus updateStatus)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == updateStatus.Id);
                if (order == null)
                {
                    return _responses.ResponseNotFound("الطلب غير موجود.");
                }

                order.Status = updateStatus.Status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess("تم تحديث حالة الطلب بنجاح.", order);
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
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return _responses.ResponseNotFound("الطلب المطلوب غير موجود!");
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess<string>("تم حذف الطلب بنجاح.", null);
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
