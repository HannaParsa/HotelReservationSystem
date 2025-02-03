using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Models;
using HotelReservationSystem.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HotelReservationSystem.Pages.Rooms
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Room Room { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int roomId)
        {
            Room = await _context.Rooms.FindAsync(roomId);

            if (Room == null)
            {
                return NotFound();
            }

            // Fetch all reservations for the room
            Reservations = await _context.Reservations
                .Where(r => r.RoomId == roomId)
                .OrderBy(r => r.FromDate)
                .ToListAsync();

            return Page();
        }
    }
}
