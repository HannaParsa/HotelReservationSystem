using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class RegisterModel : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    [BindProperty]
    public User User { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
    public string Password { get; set; }

    public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {        

        // Set the default role to "Guest"
        User.Role = "Guest";

        // Create the user with the UserManager
        var result = await _userManager.CreateAsync(User, Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(User, "Guest");
            await _signInManager.SignInAsync(User, isPersistent: false);

            return RedirectToPage("/Index");
        }

        // If there were errors, add them to the ModelState
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}
