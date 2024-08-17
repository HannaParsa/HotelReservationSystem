using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Fetch the list of rooms and their reviews to display on the main page
            Rooms = await _context.Rooms
                .Include(r => r.Reviews)
                .ThenInclude(review => review.User)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAddReviewAsync(int roomId, int rating, string comment)
        {
            var userId = HttpContext.Session.GetString("Username"); // Assuming session holds the Username
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userId);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room == null)
            {
                return NotFound();
            }

            var review = new Review
            {
                UserId = user.UserId.ToString(),
                RoomId = roomId,
                Rating = rating,
                Comment = comment
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
