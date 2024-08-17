using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public Room Room { get; set; }
        public int UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int roomId)
        {
            var userId = HttpContext.Session.GetString("Username");
            Room = await _context.Rooms.FindAsync(roomId);

            if (Room == null)
            {
                return NotFound();
            }
            UserId = Int32.Parse(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int roomId, int userId, int totalAmount)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var reservation = new Reservation
            {
                UserId = userId,
                RoomId = roomId,
                TotalAmount = totalAmount,
                Status = "Confirmed"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Index");
        }
    }
}
