using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Admin
{
    public class CreateRoomModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateRoomModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();

            return RedirectToPage("./RoomsIndex");
        }
    }
}
 