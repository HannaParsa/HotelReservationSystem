namespace HotelReservationSystem.Models
{
    public class ReservePool
    {
        public int ReservePoolId { get; set; }
        public int UserId { get; set; } //Foreign key to User
        public User User { get; set; }
        public DateTime? FromDate { get; set; }//For reservation date
        public DateTime? ToDate { get; set; }
    }
}
