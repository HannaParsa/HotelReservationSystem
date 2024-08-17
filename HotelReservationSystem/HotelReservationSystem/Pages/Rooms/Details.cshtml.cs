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

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Room Room { get; set; }
        public IList<Review> Reviews { get; set; }

        [BindProperty]
        public Review Review { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Room = await _context.Rooms.FindAsync(id);
            Reviews = await _context.Reviews
                                    .Include(r => r.User)
                                    .Where(r => r.RoomId == id)
                                    .ToListAsync();

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

            Review.ReviewDate = DateTime.Now;
            Review.UserId = 1;
            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = Review.RoomId });
        }
    }
}
