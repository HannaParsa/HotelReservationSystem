using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelReservationSystem.Models;
using HotelReservationSystem.Data;
using System.Linq;

namespace HotelReservationSystem.Pages.Reservations
{
    public class ConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Room Room { get; set; }

        public ConfirmationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int roomId)
        {
            Room = _context.Rooms.FirstOrDefault(m => m.RoomId == roomId);

            if (Room == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
