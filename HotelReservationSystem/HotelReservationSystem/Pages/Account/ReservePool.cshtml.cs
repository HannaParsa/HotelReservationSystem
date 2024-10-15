using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

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
            var username = HttpContext.Session.GetString("Username");
            var pool = await _context.Pools.FindAsync(poolId);
            var user = _context.Users.FirstOrDefault(x => x.Username == username);

            if (pool == null || user == null)
            {
                return NotFound();
            }

            // Check if the pool is available
            if (!IsPoolAvailable(poolId, FromDate, ToDate))
            {
                return NotFound("The selected date range is not available.");
            }
            var roomResrvations = _context.Reservations.Where(x => x.UserId == user.UserId).ToList();
            for(int i = 0; i < roomResrvations.Count(); i++)
            {
                if (FromDate >= roomResrvations[i].FromDate && ToDate <= roomResrvations[i].ToDate)
                    break;
                else if((FromDate < roomResrvations[i].FromDate || ToDate > roomResrvations[i].ToDate) &&
                    i == (roomResrvations.Count()) - 1)
                {
                    return NotFound("The selected date range is not in your Room reservation date.");
                }    
            }
            var newPool = new Pool
            {
                IsAvailable = false, 
                ImageURL = pool.ImageURL,  
                FromDate = FromDate,  
                ToDate = ToDate,    
            };

            _context.Pools.Add(newPool);
            await _context.SaveChangesAsync();

            var reservation = new ReservePool
            {
                PoolId = newPool.PoolId,  
                FromDate = FromDate,
                ToDate = ToDate,
                UserId = user.UserId
            };
            
            _context.ReservePools.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Confirmation", new { poolId = newPool.PoolId });
        }

        private bool IsPoolAvailable(int poolId, DateTime fromDate, DateTime toDate)
        {
            var reservations = _context.ReservePools
                .Select(x => x)
                .ToList();

            foreach (var reservation in reservations)
            {
                if ((fromDate < reservation.ToDate && fromDate >= reservation.FromDate) ||
                    (toDate > reservation.FromDate && toDate <= reservation.ToDate) ||
                    (fromDate <= reservation.FromDate && toDate >= reservation.ToDate))
                {
                    return false; // Pool is not available
                }
            }

            return true; // Pool is available
        }
    }
}
