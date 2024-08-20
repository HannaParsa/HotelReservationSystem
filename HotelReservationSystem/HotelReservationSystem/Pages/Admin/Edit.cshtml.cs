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

            var userToUpdate = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == User.Username);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Update user properties
            userToUpdate.Email = User.Email;
            userToUpdate.Role = User.Role;

            if (!string.IsNullOrEmpty(User.Password))
            {
                userToUpdate.Password = User.Password; 
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

    }
}
