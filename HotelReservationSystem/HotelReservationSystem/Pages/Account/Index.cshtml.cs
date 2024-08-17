using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Room> Rooms { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch the list of rooms to display on the main page
            Rooms = await _context.Rooms.ToListAsync();
        }
    }
}
