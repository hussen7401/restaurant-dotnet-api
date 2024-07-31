using core.DTOs.TableDto;
using core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : Controller
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTables()
        {
            return await _tableService.GetAllTables();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTableById(int id)
        {
            return await _tableService.GetTableById(id);
        }
        [HttpPost("Create")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateTable([FromBody] TableDto tableDto)
        {
            return await _tableService.CreateTable(tableDto);
        }
        [HttpPut("update/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateTable(int id, [FromBody] TableDto tableDto)
        {
            return await _tableService.UpdateTable(id, tableDto);
        }
        [HttpDelete("delete/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTable(int id)
        {
            return await _tableService.DeleteTable(id);
        }
    }
}
