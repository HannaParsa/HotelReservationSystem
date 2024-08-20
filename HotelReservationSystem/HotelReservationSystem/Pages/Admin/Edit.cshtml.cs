using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var userToUpdate = await _context.Users.FindAsync(User.Username);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Update properties
            userToUpdate.Username = User.Username;
            userToUpdate.Email = User.Email;
            userToUpdate.Role = User.Role;
            // Optionally handle password update separately
            if (!string.IsNullOrEmpty(User.Password))
            {
                userToUpdate.Password = User.Password; // Consider hashing the password here
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToPage("Admin/Edit");
            }

            return RedirectToPage("./Index");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
