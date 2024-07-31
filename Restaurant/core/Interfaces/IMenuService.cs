using core.Entities;
using Microsoft.AspNetCore.Mvc;
using core.DTOs.MenuItemDto;


namespace core.Interfaces
{
    public interface IMenuService
    {
        Task<ActionResult> GetAllMenuItems();
        Task<ActionResult> GetMenuItemById(int id);
        Task<ActionResult> GetMenuItemByName(string name);
        Task<ActionResult> CreateMenuItem(MenuItemDto menuItemDto);
        Task<ActionResult> UpdateMenuItem(int id, MenuItemDto menuItemDto);
        Task<ActionResult> DeleteMenuItem(int id);
    }
}
