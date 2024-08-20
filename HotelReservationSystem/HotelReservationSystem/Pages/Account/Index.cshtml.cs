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
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string Status { get; set; }

        public async Task OnGetAsync(int? minPrice, int? maxPrice, string status)
        {

            MinPrice = minPrice;
            MaxPrice = maxPrice;
            Status = status;

            var query = _context.Rooms.Include(r => r.Reviews).AsQueryable();

            // price filter
            if (MinPrice.HasValue)
            {
                query = query.Where(r => r.Price >= MinPrice.Value);
            }

            if (MaxPrice.HasValue)
            {
                query = query.Where(r => r.Price <= MaxPrice.Value);
            }

            // status filter
            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "available")
                {
                    query = query.Where(r => r.IsAvailable);
                }
                else if (Status == "notavailable")
                {
                    query = query.Where(r => !r.IsAvailable);
                }
            }

            Rooms = await query.ToListAsync();
        }

        public async Task<IActionResult> OnPostAddReviewAsync(int roomId, int rating, string comment)
        {
            var username = HttpContext.Session.GetString("Username");

            if (username == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Retrieve the user by Username or UserId
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                // Handle the case where the user does not exist
                return RedirectToPage("/Account/Login");
            }

            // Retrieve the room
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room == null)
            {
                return NotFound();
            }

            var review = new Review
            {
                UserId = user.UserId,
                RoomId = roomId,
                Rating = rating,
                Comment = comment,
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Index");
        }

    }
}
