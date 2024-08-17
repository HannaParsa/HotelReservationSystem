namespace HotelReservationSystem.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; } //Foreign key to User
        public User User { get; set; } 
        public int RoomId { get; set; } //Foreign key to Room
        public Room Room { get; set; } 
        public int Rating { get; set; } //Rating out of 5
        public string Comment { get; set; }
    }
}
