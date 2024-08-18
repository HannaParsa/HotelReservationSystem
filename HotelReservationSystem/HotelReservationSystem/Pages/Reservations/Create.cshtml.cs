using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelReservationSystem.Models;
using HotelReservationSystem.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Room Room { get; set; }

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            Room = _context.Rooms.FirstOrDefault(m => m.RoomId == roomId);

            if (Room == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostReserveAsync(int roomId)
        {
            // Replace this with your method of retrieving the current user's ID
            var username = HttpContext.Session.GetString("Username");

            // Retrieve the user from your custom User table
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return Unauthorized(); // Or redirect to login page
            }

            // Retrieve the room
            var room = _context.Rooms.FirstOrDefault(m => m.RoomId == roomId);

            if (room == null || !room.IsAvailable)
            {
                return NotFound("The selected room is not available.");
            }

            // Create a new reservation
            var reservation = new Reservation
            {
                UserId = user.UserId,
                RoomId = room.RoomId,
                TotalAmount = room.Price,  // Or calculate based on nights, etc.
                Status = "Confirmed"
            };

            // Save the reservation to the database
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Redirect to a confirmation page
            return RedirectToPage("/Account/Index");
        }

        // Helper method to get the current user's ID - replace with your logic
        private int GetCurrentUserId()
        {
            // This is a placeholder. Implement your logic to get the currently logged-in user's ID.
            // For example, you might store the user ID in a session or retrieve it from a JWT token.
            return int.Parse(User.Identity.Name); // Assuming User.Identity.Name contains the user ID.
        }
    }
}
