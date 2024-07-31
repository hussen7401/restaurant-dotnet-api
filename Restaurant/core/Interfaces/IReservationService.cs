using core.Entities;
using Microsoft.AspNetCore.Mvc;
using core.DTOs.ReservationDto;

namespace core.Interfaces
{
    public interface IReservationService
    {
        Task<ActionResult> GetAllReservations();
        Task<ActionResult> GetReservationsByUserId(int userId);
        Task<ActionResult> GetReservationById(int id);
        Task<ActionResult> CreateReservation(CreateReservationDto CreateR);
        Task<ActionResult> UpdateReservation(int id, UpdateReservationDto UpdateR);
        Task<ActionResult> DeleteReservation(int id);
    }
}
