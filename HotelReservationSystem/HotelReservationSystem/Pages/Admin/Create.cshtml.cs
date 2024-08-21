using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public User User { get; set; }

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            // Add the new user to the database
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            // Redirect to the Admin Index page
            return RedirectToPage("./Index");
        }
    }
}
