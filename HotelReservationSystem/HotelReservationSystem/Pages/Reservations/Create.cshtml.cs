﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelReservationSystem.Models;
using HotelReservationSystem.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages.Reservations
{
    public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public Room Room { get; set; }

    // Add properties for FromDate and ToDate
    [BindProperty]
    public DateTime? FromDate { get; set; }
    
    [BindProperty]
    public DateTime? ToDate { get; set; }

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet(int? roomId)
    {
        if (roomId == null)
        {
            return NotFound();
        }

        Room = _context.Rooms.FirstOrDefault(m => m.RoomId == roomId);

        if (Room == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostReserveAsync(int roomId)
    {
        var username = HttpContext.Session.GetString("Username");

        // Retrieve the user
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            return Unauthorized(); 
        }

        // Retrieve the room
        var room = _context.Rooms.FirstOrDefault(m => m.RoomId == roomId);

        if (room == null || !room.IsAvailable)
        {
            return NotFound("The selected room is not available.");
        }

        // Check if the dates are valid
        if (FromDate == null || ToDate == null || FromDate >= ToDate)
        {
            ModelState.AddModelError("", "Invalid reservation dates.");
            return Page();
        }

        // Create a new reservation
        var reservation = new Reservation
        {
            UserId = user.UserId,
            RoomId = room.RoomId,
            TotalAmount = room.Price,
            Status = "Confirmed",
            FromDate = FromDate.Value,
            ToDate = ToDate.Value
        };

        // Make room unavailable for other reservations
        room.IsAvailable = false;
        room.FromDate = FromDate;
        room.ToDate = ToDate;

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        // Redirect to a confirmation page
        return RedirectToPage("/Reservations/Confirmation", new { roomId = roomId });
    }
}

}
