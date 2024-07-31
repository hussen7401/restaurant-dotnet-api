using core.DTOs.TableDto;
using Microsoft.AspNetCore.Mvc;

namespace core.Interfaces
{
    public interface ITableService
    {
        Task<ActionResult> GetAllTables();
        Task<ActionResult> GetTableById(int id);
        Task<ActionResult> CreateTable(TableDto tableDto);
        Task<ActionResult> UpdateTable(int id, TableDto tableDto);
        Task<ActionResult> DeleteTable(int id);
    }
}
