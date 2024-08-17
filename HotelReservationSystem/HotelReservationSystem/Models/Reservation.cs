namespace HotelReservationSystem.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string UserId { get; set; } // Foreign key to User
        public User User { get; set; } // Navigation property
        public int RoomId { get; set; } // Foreign key to Room
        public Room Room { get; set; } // Navigation property
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // e.g., Confirmed, Cancelled, Completed
    }
}
