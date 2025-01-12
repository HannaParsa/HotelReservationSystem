using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelReservationSystem.Pages.Errors
{
     public class ErrorModel : PageModel
     {
         [BindProperty(SupportsGet = true)]
         public string ErrorMessage { get; set; }

         public void OnGet()
         {
             // ErrorMessage will be set via query string.
         }
     }
 }
