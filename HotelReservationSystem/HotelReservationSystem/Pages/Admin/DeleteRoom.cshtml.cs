using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Pages.Admin.Rooms
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room = await _context.Rooms.FindAsync(id);

            if (Room == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existingRoom = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomNumber == Room.RoomNumber);

            if (existingRoom == null)
            {
                return NotFound();
            }


            if (existingRoom != null)
            {
                _context.Rooms.Remove(existingRoom);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
