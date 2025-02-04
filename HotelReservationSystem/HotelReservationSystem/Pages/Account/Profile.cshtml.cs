using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HotelReservationSystem.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        public IList<Reservation> Reservations { get; set; }
        public IList<ReservePool> PoolReservations { get; set; }  
        public IList<Room> Rooms { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Account/Login");
            }

            User = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (User == null)
            {
                return NotFound();
            }

            // Get user's reservations 
            Reservations = await _context.Reservations
                .Include(r => r.Room) 
                .Where(r => r.UserId == User.UserId)
                .ToListAsync();

            // Get pool reservations 
            PoolReservations = await _context.ReservePools
                .Where(p => p.UserId == User.UserId)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var oldUsername = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(oldUsername))
            {
                return RedirectToPage("/Account/Login");
            }

            var userFromDb = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == oldUsername);

            if (userFromDb == null)
            {
                return NotFound();
            }

            userFromDb.Username = User.Username;
            userFromDb.Email = User.Email;

            if (!string.IsNullOrEmpty(User.Password))
            {
                userFromDb.Password = User.Password;
            }

            await _context.SaveChangesAsync();

            if (oldUsername != User.Username)
            {
                HttpContext.Session.SetString("Username", User.Username);
            }

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostRemoveReservationAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Update the room's availability
            var room = await _context.Rooms
                .Where(x => x.RoomId == reservation.RoomId).FirstOrDefaultAsync();
            room.IsAvailable = true;
            room.ToDate = null;
            room.FromDate = null;

            // Remove the reservation
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemovePoolReservationAsync(int id)
        {
            var poolReservation = await _context.ReservePools
                .FirstOrDefaultAsync(pr => pr.ReservePoolId == id);            

            if (poolReservation == null)
            {
                return NotFound();
            }

            // Remove the pool reservation
            _context.ReservePools.Remove(poolReservation);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
