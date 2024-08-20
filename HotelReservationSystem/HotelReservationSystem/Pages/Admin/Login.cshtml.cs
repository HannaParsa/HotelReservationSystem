using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AdminLoginModel Admin { get; set; }

        

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == Admin.Username && u.Password == Admin.Password);

            if (user == null || user.Role != "Admin")
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            HttpContext.Session.SetString("AdminUsername", user.Username);

            return RedirectToPage("/Admin/Index");
        }
    }
    public class AdminLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
