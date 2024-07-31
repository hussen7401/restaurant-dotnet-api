using core.DTOs.ReservationDto;
using core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult> GetReservations()
        {
            return await _reservationService.GetAllReservations();
        }

        // GET: api/Reservations/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetReservationsByUserId(int userId)
        {
            return await _reservationService.GetReservationsByUserId(userId);
        }

        // GET: api/Reservations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetReservation(int id)
        {
            return await _reservationService.GetReservationById(id);
        }

        // POST: api/Reservations/create
        [HttpPost("create")]
        public async Task<ActionResult> CreateReservation([FromBody] CreateReservationDto reservationDto)
        {
            return await _reservationService.CreateReservation(reservationDto);
        }

        // PUT: api/Reservations/update/{id}
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateReservation(int id, [FromBody] UpdateReservationDto reservationDto)
        {
            return await _reservationService.UpdateReservation(id, reservationDto);
        }

        // DELETE: api/Reservations/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            return await _reservationService.DeleteReservation(id);
        }
    }
}
