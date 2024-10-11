using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelReservationSystem.Pages.Account
{
    public class ConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Pool Pool { get; set; }

        public ConfirmationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int poolId)
        {
            Pool = _context.Pools.FirstOrDefault(m => m.PoolId == poolId);

            if (Pool == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
