using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Models;
using HotelReservationSystem.Data;

namespace HotelReservationSystem.Pages.Rooms
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Room Room { get; set; }

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

            return Page();
        }
    }
}
