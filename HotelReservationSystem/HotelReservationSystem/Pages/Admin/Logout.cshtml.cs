using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelReservationSystem.Pages
{
    public class AdminLogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("AdminUser");
            return RedirectToPage("/AdminLogin");
        }
    }
}
