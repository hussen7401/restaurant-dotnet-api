using core.DTOs.MenuItemDto;
using core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] 
    public class MenuItemController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuItemController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/MenuItems
        [HttpGet]
        public async Task<ActionResult> GetMenuItems()
        {
            return await _menuService.GetAllMenuItems();
        }

        // GET: api/MenuItems/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMenuItem(int id)
        {
            return await _menuService.GetMenuItemById(id);
        }

        // GET: api/MenuItems/name/{name}
        [HttpGet("name/{name}")]
        public async Task<ActionResult> SearchMenuItemByName(string name)
        {
            return await _menuService.GetMenuItemByName(name);
        }

        // POST: api/MenuItems
        [HttpPost("create")]
        //[Authorize(Roles = "Admin")] 
        public async Task<ActionResult> CreateMenuItem([FromBody] MenuItemDto menuItemDto)
        {
            return await _menuService.CreateMenuItem(menuItemDto);
        }

        // PUT: api/MenuItems/update/{id}
        [HttpPut("update/{id}")]
        //[Authorize(Roles = "Admin")] 
        public async Task<ActionResult> UpdateMenuItem(int id, [FromBody] MenuItemDto menuItemDto)
        {
            return await _menuService.UpdateMenuItem(id, menuItemDto);
        }

        // DELETE: api/MenuItems/delete/{id}
        [HttpDelete("delete/{id}")]
        //[Authorize(Roles = "Admin")] 
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
            return await _menuService.DeleteMenuItem(id);
        }
    }
}
