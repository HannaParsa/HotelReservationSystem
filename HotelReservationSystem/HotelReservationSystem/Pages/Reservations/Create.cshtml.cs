using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelReservationSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HotelReservationSystem.Data;

namespace HotelReservationSystem.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public CreateModel(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public class RegisterInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            
            [Required]
            public int RoomId { get; set; }           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = Input.Email,
                    Email = Input.Email,
                };
                
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var reservation = new Reservation
                    {
                        UserId = user.UserId
                    };

                    _context.Reservations.Add(reservation);
                    await _context.SaveChangesAsync();

                    return RedirectToPage("/Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
