using AutoMapper;
using core.DTOs.ReservationDto;
using core.Entities;
using core.Interfaces;
using core.Helpers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly Responses _responses;

        public ReservationService(MyDbContext context, IMapper mapper, Responses responses)
        {
            _context = context;
            _mapper = mapper;
            _responses = responses;
        }
        public async Task<ActionResult> GetAllReservations()
        {
            try
            {
                var reservations = await _context.Reservations
                    .Include(r => r.User)
                    .ToListAsync();

                if (reservations == null || !reservations.Any())
                {
                    return _responses.ResponseNotFound("لا توجد أي حجوزات !");
                }
                var reservationDtos = _mapper.Map<List<ReservationDto>>(reservations);
                return _responses.ResponseSuccess("تم جلب الحجوزات بنجاح.", reservationDtos);
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
        public async Task<ActionResult> GetReservationsByUserId(int userId)
        {
            try
            {
                var reservations = await _context.Reservations
                    .Where(r => r.UserId == userId)
                    .Include(r => r.User)
                    .ToListAsync();


                if (reservations == null || !reservations.Any())
                {
                    return _responses.ResponseNotFound("لا توجد حجوزات لهذا المستخدم!");
                }
                var reservationDtos = _mapper.Map<List<ReservationDto>>(reservations);

                return _responses.ResponseSuccess("تم جلب حجوزات المستخدم بنجاح.", reservationDtos);
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
        public async Task<ActionResult> GetReservationById(int id)
        {
            try
            {
                var reservation = await _context.Reservations
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (reservation == null)
                {
                    return _responses.ResponseNotFound("الحجز المطلوب غير متوفر!");
                }

                var reservationDto = _mapper.Map<ReservationDto>(reservation);
                return _responses.ResponseSuccess("تم جلب بيانات الحجز بنجاح.", reservationDto);
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
        public async Task<ActionResult> CreateReservation(CreateReservationDto CreateR)
        {
            if (CreateR == null)
            {
                return _responses.ResponseBadRequest("بيانات الحجز غير صالحة!");
            }

            if (CreateR.NumberOfGuests <= 0 || CreateR.NumberOfGuests > 10)
            {
                return _responses.ResponseBadRequest("عدد الضيوف يجب أن يكون بين 1 و 10.");
            }

            var openingTime = new TimeSpan(8, 0, 0);
            var closingTime = new TimeSpan(24, 0, 0);
            var reservationTime = CreateR.ReservationDate.TimeOfDay;

            if (reservationTime < openingTime || reservationTime >= closingTime)
            {
                return _responses.ResponseBadRequest("الوقت المحدد للحجز غير متاح. الرجاء اختيار وقت بين الساعة 8 صباحًا و12 مسائأ.");
            }

            try
            {
                var existingReservation = await _context.Reservations
                    .Where(r => r.TableId == CreateR.TableId &&
                                r.ReservationDate.Date == CreateR.ReservationDate.Date &&
                                r.ReservationDate.Hour == CreateR.ReservationDate.Hour)
                    .FirstOrDefaultAsync();

                if (existingReservation != null)
                {
                    return _responses.ResponseConflict("الطاولة محجوزة في هذا الوقت. الرجاء اختيار وقت آخر.");
                }

                var reservation = _mapper.Map<Reservations>(CreateR);
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess("تم إنشاء الحجز بنجاح.", reservation);
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
        public async Task<ActionResult> UpdateReservation(int id, UpdateReservationDto UpdateR)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null)
                {
                    return _responses.ResponseNotFound("الحجز المطلوب غير متوفر!");
                }

                if (UpdateR.NumberOfGuests <= 0 || UpdateR.NumberOfGuests > 10)
                {
                    return _responses.ResponseBadRequest("عدد الضيوف يجب أن يكون بين 1 و 10.");
                }

                _mapper.Map(UpdateR, reservation);
                reservation.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess("تم تحديث الحجز بنجاح.", reservation);
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
        public async Task<ActionResult> DeleteReservation(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null)
                {
                    return _responses.ResponseNotFound("الحجز المطلوب غير متوفر!");
                }

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();

                return _responses.ResponseSuccess<string>("تم حذف الحجز بنجاح.", null);
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
