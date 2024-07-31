using AutoMapper;
using core.DTOs.MenuItemDto;
using core.Entities;
using core.Helpers;
using core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace core.Servises
{
    public class MenuService : IMenuService
    {
        private readonly MyDbContext _context;
        private readonly Responses _responses;
        private readonly IMapper _mapper;

        public MenuService(MyDbContext context, Responses responses, IMapper mapper)
        {
            _context = context;
            _responses = responses;
            _mapper = mapper;
        }
        public async Task<ActionResult> GetAllMenuItems()
        {
            try
            {
                var ListMenuItem = await _context.MenuItems.ToListAsync();

                if (ListMenuItem == null || !ListMenuItem.Any())
                {
                    return _responses.ResponseNotFound("لا توجد بيانات للعرض!");
                }

                return _responses.ResponseSuccess("تم جلب البيانات بنجاح.", ListMenuItem);
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
        public async Task<ActionResult> GetMenuItemById(int id)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(id);

                if (menuItem == null)
                {
                    return _responses.ResponseNotFound("الوجبة المطلوبة غير موجودة!");
                }

                return _responses.ResponseSuccess<MenuItem>("تم جلب بيانات الوجبة بنجاح", menuItem);
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
        public async Task<ActionResult> GetMenuItemByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return _responses.ResponseBadRequest("اسم الوجبة مطلوب!");
            }
            try
            {
                var menuItem = await _context.MenuItems.FirstOrDefaultAsync(mi => mi.Name == name);

                if (menuItem == null)
                {
                    return _responses.ResponseNotFound("الوجبة المطلوبة غير موجودة!");
                }

                if (!menuItem.IsAvailable)
                {
                    return _responses.ResponseUnavailable("الوجبة غير متوفرة حاليا.");
                }

                return _responses.ResponseSuccess("تم جلب بيانات الوجبة بنجاح.", menuItem);
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
        public async Task<ActionResult> CreateMenuItem(MenuItemDto menuItemDto)
        {
            if (menuItemDto == null || string.IsNullOrWhiteSpace(menuItemDto.Name) || string.IsNullOrWhiteSpace(menuItemDto.Description) || menuItemDto.Price <= 0)
            {
                return _responses.ResponseBadRequest("بيانات الوجبة غير مكتملة أو غير صحيحة!");
            }
            try
            {
                var menuItem = _mapper.Map<MenuItem>(menuItemDto);
                await _context.MenuItems.AddAsync(menuItem);
                await _context.SaveChangesAsync();
                return _responses.ResponseSuccess<MenuItem>("تم إنشاء الوجبة بنجاح.", menuItem);

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
        public async Task<ActionResult> UpdateMenuItem(int id, MenuItemDto menuItemDto)
        {
            if (menuItemDto == null || string.IsNullOrWhiteSpace(menuItemDto.Name) || string.IsNullOrWhiteSpace(menuItemDto.Description) || menuItemDto.Price <= 0)
            {
                return _responses.ResponseBadRequest("بيانات الوجبة غير مكتملة أو غير صحيحة!");
            }

            try
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem == null)
                {
                    return _responses.ResponseNotFound("الوجبة المطلوبة غير موجودة!");
                }

                _mapper.Map(menuItemDto, menuItem);
                menuItem.UpdatedAt = DateTime.UtcNow;
                _context.MenuItems.Update(menuItem);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess<MenuItem>("تم تحديث بيانات الوجبة بنجاح.", menuItem);
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
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem == null)
                {
                    return _responses.ResponseNotFound("الوجبة المطلوبة غير موجودة!");
                }

                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess("تم حذف الوجبة بنجاح.");
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