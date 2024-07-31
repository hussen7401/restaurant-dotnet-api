using System.ComponentModel.DataAnnotations;

namespace core.DTOs.ReservationDto
{
    public class CreateReservationDto
    {
        public int UserId { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string? SpecialRequest { get; set; }
    }
}
