using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class RegisterModel : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    [BindProperty]
    public User User { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
    public string Password { get; set; }

    public RegisterModel(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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

        // Set the default role to "Guest"
        User.Role = "Guest";

        // Hash the password
        User.Password = HashPassword(Password);

        // Add the user to the database
        _dbContext.Users.Add(User);
        await _dbContext.SaveChangesAsync();

        // Sign in the user
        // Note: Manual sign-in process should be implemented here, e.g., by setting a cookie or using a token.

        return RedirectToPage("/Index");
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
