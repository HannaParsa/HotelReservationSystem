using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Pages.Admin
{
    public class EditRoomModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditRoomModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                Room = new Room();
                return Page();
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Room.RoomId == 0)
            {
                _context.Rooms.Add(Room);
            }
            else
            {
                _context.Attach(Room).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
