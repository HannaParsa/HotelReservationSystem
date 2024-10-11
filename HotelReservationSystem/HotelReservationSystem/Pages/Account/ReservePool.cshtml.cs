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
            var username = HttpContext.Session.GetString("Username");
            var pool = await _context.Pools.FindAsync(poolId);
            var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();
            if (pool == null )
            {
                return Page();
            }
            if(pool.IsAvailable == false)
            {
                if(pool.FromDate< ToDate && pool.ToDate< FromDate)
                {
                    var reservationAfter = new ReservePool
                    {
                        PoolId = pool.PoolId,
                        FromDate = FromDate,
                        ToDate = ToDate,
                        UserId = user.UserId
                    };
                    pool.IsAvailable = false;
                    pool.FromDate = reservationAfter.FromDate;
                    pool.ToDate = reservationAfter.ToDate;

                    _context.ReservePools.Add(reservationAfter);
                    await _context.SaveChangesAsync();

                    return RedirectToPage("/Account/Confirmation", new { poolId });
                }
                else
                {
                    return NotFound("The selected date pool is not available.");
                }
            };
             var reservation = new ReservePool
            {
                PoolId = pool.PoolId,
                FromDate = FromDate,
                ToDate = ToDate,
                UserId = user.UserId
            };
            pool.IsAvailable = false;
            pool.FromDate = reservation.FromDate;
            pool.ToDate = reservation.ToDate;

            _context.ReservePools.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Confirmation", new { poolId });
        }
    }

}
