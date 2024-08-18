using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelReservationSystem.Models;  
using HotelReservationSystem.Data;

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

        public IActionResult OnPostReserve(int roomId)
        {
            // Logic to reserve the room (e.g., save reservation to the database)
            // After reserving, you can redirect to a confirmation page or elsewhere
            return RedirectToPage("/Reservations/Confirmation", new { roomId = roomId });
        }
    }
}