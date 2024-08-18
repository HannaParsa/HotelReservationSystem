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
                return Unauthorized(); 
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
                TotalAmount = room.Price,  
                Status = "Confirmed"
            };
            //make room unavailable for other reservations
            room.IsAvailable = false;

            // Save the reservation to the database
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Redirect to a confirmation page
            return RedirectToPage("/Reservations/Confirmation", new { roomId = roomId });
        }

    }
}
