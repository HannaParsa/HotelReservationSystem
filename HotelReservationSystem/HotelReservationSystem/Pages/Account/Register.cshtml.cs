using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Username { get; set; }    

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            // Check if the user already exists
            var userExists = _context.Users.Any(u => u.Email == Input.Email);
            if (userExists)
            {
                ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                return RedirectToPage("/Account/Index");
            }

            // Create new user
            var newUser = new User
            {
                Email = Input.Email,
                Password = Input.Password,
                Username = Input.Username,
                Role = "Guest"
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Redirect to login page
            return RedirectToPage("/Account/Index");
        }
    }
}
