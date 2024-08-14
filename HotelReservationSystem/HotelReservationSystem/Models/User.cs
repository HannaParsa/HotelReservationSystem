using Microsoft.AspNetCore.Identity;

namespace HotelReservationSystem.Models
{
    public class User: IdentityUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Guest or Admin
        public string Email { get; set; }
    }
}
