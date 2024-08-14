using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Rooms
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Room Room { get; set; }
        public IList<Review> Reviews { get; set; }

        public async Task OnGetAsync(int id)
        {
            Room = await _context.Rooms.FirstOrDefaultAsync(m => m.RoomId == id);
            Reviews = await _context.Reviews.Where(r => r.RoomId == id).ToListAsync();
        }
    }
}
