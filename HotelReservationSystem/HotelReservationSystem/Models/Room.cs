namespace HotelReservationSystem.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; } //Single, Double, Suite
        public int Price { get; set; }
        public bool IsAvailable { get; set; } //if the room is currently available
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
