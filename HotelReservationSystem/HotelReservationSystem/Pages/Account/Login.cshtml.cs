using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // For HttpContext.Session
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check user 
            var user = _context.Users
                .FirstOrDefault(u => u.Username == Input.Username && u.Password == Input.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // Store the Username in the session
            HttpContext.Session.SetString("Username", user.Username);

            // Redirect to a page after successful login
            return RedirectToPage("/Account/Index");
        }
    }
}
