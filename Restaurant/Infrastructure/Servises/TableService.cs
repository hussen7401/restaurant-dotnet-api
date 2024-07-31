using AutoMapper;
using core.DTOs.TableDto;
using core.Entities;
using core.Helpers;
using core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Servises
{
    public class TableService : ITableService
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly Responses _responses;

        public TableService(MyDbContext context, IMapper mapper, Responses responses)
        {
            _context = context;
            _mapper = mapper;
            _responses = responses;
        }
        public async Task<ActionResult> GetAllTables()
        {
            try
            {
                var tables = await _context.Tables.ToListAsync();

                if (tables == null)
                {
                    return _responses.ResponseNotFound("لا توجد طاولات متاحة!");
                }

                return _responses.ResponseSuccess("تم جلب الطاولات بنجاح.", tables);
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
        public async Task<ActionResult> GetTableById(int id)
        {
            try
            {
                var table = await _context.Tables.FindAsync(id);

                if (table == null)
                {
                    return _responses.ResponseNotFound("الطاولة المطلوبة غير متوفرة!");
                }

                return _responses.ResponseSuccess("تم جلب بيانات الطاولة بنجاح.", table);
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
        public async Task<ActionResult> CreateTable(TableDto tableDto)
        {
            if (tableDto == null)
            {
                return _responses.ResponseBadRequest("بيانات الطاولة غير صالحة!");
            }

            try
            {
                var table = new RestaurantTable()
                {
                    Capacity = tableDto.Capacity,
                    TableNumber = tableDto.TableNumber,
                };
                _context.Tables.Add(table);
                await _context.SaveChangesAsync();
                return _responses.ResponseSuccess("تم إنشاء الطاولة بنجاح.");
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
        public async Task<ActionResult> UpdateTable(int id, TableDto tableDto)
        {
            try
            {
                var table = await _context.Tables.FindAsync(id);

                if (table == null)
                {
                    return _responses.ResponseNotFound("الطاولة المطلوبة غير متوفرة!");
                }

                _mapper.Map(tableDto, table);
                table.UpdatedAt = DateTime.UtcNow;
                _context.Tables.Update(table);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess("تم تحديث الطاولة بنجاح.", table);
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
        public async Task<ActionResult> DeleteTable(int id)
        {
            try
            {
                var table = await _context.Tables.FindAsync(id);

                if (table == null)
                {
                    return _responses.ResponseNotFound("الطاولة المطلوبة غير متوفرة!");
                }

                _context.Tables.Remove(table);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess<string>("تم حذف الطاولة بنجاح.", null);
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
