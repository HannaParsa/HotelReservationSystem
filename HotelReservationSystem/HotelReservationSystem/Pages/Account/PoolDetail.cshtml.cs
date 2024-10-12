using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Account
{
    public class PoolDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Pool Pool { get; set; } 
        public List<ReservePool> ReservePools { get; set; } 

        public PoolDetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? poolId)
        {
            if (poolId == null)
            {
                return NotFound();
            }

            Pool = await _context.Pools.FindAsync(poolId);

            if (Pool == null)
            {
                return NotFound();
            }

            // Fetch all pools to display below the first pool
            ReservePools = await _context.ReservePools.ToListAsync();

            return Page();
        }
    }
}
