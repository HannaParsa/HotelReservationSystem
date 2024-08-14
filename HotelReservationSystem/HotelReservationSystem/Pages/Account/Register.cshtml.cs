using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModel : PageModel
{
    private readonly ApplicationDbContext _context;

    [BindProperty]
    public User User { get; set; }

    public RegisterModel(ApplicationDbContext context)
    {
        _context = context;
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

        _context.Users.Add(User);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}
