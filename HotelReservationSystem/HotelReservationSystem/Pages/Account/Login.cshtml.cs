using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using HotelReservationSystem.Models;
using System.Threading.Tasks;
using HotelReservationSystem.Data;

namespace HotelReservationSystem.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(Input.UserName);

                    if (user == null)
                    {
                        user = new User
                        {
                            Username = Input.UserName,
                            Email = Input.Email,
                            Role = "Guest" // Or other role logic
                        };

                        var createResult = await _userManager.CreateAsync(user, Input.Password);

                        if (createResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                    }

                    return RedirectToPage("/Rooms/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return Page();
        }
    }

    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Email { get; set; }
    }
}
