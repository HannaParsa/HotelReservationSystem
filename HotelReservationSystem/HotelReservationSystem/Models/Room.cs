namespace HotelReservationSystem.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; } // e.g., Single, Double, Suite
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } // Indicates if the room is currently available
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
}
