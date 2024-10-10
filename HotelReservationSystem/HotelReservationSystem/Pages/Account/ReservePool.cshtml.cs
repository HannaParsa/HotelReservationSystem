using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelReservationSystem.Pages.Account
{
    public class ReservePoolModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public DateTime FromDate { get; set; }
        [BindProperty]
        public DateTime ToDate { get; set; }
        public Pool Pool { get; set; }

        public ReservePoolModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int poolId)
        {
            Pool = await _context.Pools.FindAsync(poolId);
            if (Pool == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int poolId)
        {
            var pool = await _context.Pools.FindAsync(poolId);
            if (pool == null || !ModelState.IsValid)
            {
                return Page();
            }

            var reservation = new ReservePool
            {
                PoolId = pool.PoolId,
                FromDate = FromDate,
                ToDate = ToDate,
                UserId = /* current user id */ 1 // Use actual logged-in user ID here
            };

            _context.ReservePools.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Pools/Details", new { poolId });
        }
    }

}
