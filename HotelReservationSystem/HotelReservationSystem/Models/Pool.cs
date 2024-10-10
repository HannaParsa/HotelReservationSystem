namespace HotelReservationSystem.Models
{
    public class Pool
    {
        public int PoolId { get; set; }
        public bool IsAvailable { get; set; } //if the pool is currently available
        public string ImageURL { get; set; }
        public DateTime? FromDate { get; set; }//For reservation date
        public DateTime? ToDate { get; set; }
    }
}
