using core.DTOs.AccountDto;

namespace core.DTOs.ReservationDto
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string? SpecialRequest { get; set; }

        public UserInfoDto? UserInfo { get; set; }
    }
}
