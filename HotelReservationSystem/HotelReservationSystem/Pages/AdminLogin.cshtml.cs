using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages
{
    public class AdminLoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminLoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginModel LoginModel { get; set; }

        

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == LoginModel.Username && u.Password == LoginModel.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            if (user.Role != "Admin")
            {
                ModelState.AddModelError(string.Empty, "You are not authorized to access the admin panel.");
                return Page();
            }

            // Set a session or cookie to mark the user as logged in
            HttpContext.Session.SetString("AdminUser", user.Username);

            return RedirectToPage("/Admin/Index");
        }
    }
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
