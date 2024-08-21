using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
           
            var existingRoom = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomNumber == Room.RoomNumber);

            if (existingRoom == null)
            {
                return NotFound();
            }

            // Update the room details
            existingRoom.RoomType = Room.RoomType;
            existingRoom.Price = Room.Price;
            existingRoom.IsAvailable = Room.IsAvailable;
            existingRoom.Description = Room.Description;
            existingRoom.ImageURL = Room.ImageURL;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToPage("Admin/EditRoom");
            }

            return RedirectToPage("./RoomsIndex");
        }

    }
}
