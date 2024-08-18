using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HotelReservationSystem.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve the username from session or another authentication mechanism
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Account/Login"); // Redirect to login if username not found in session
            }

            User = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var oldUsername = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(oldUsername))
            {
                return RedirectToPage("/Account/Login"); // Redirect to login if username not found in session
            }

            var userFromDb = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == oldUsername);

            if (userFromDb == null)
            {
                return NotFound();
            }

            // Update user details from form
            userFromDb.Username = User.Username; // Update username
            userFromDb.Email = User.Email;

            // Handle password update logic
            if (!string.IsNullOrEmpty(User.Password))
            {
                userFromDb.Password = User.Password; // Hash the password in a real application
            }

            await _context.SaveChangesAsync();

            // Update the session with the new username if changed
            if (oldUsername != User.Username)
            {
                HttpContext.Session.SetString("Username", User.Username);
            }

            return RedirectToPage(); // Refresh the current page
        }
    }
}
