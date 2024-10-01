namespace HotelReservationSystem.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; } //Foreign key to User
        public User User { get; set; } 
        public int RoomId { get; set; } //Foreign key to Room
        public Room Room { get; set; } 
        public int TotalAmount { get; set; }
        public string Status { get; set; } //Confirmed, Cancelled, Completed
        public DateTime? FromDate { get; set; }//For reservation date
        public DateTime? ToDate { get; set; }
    }
}
