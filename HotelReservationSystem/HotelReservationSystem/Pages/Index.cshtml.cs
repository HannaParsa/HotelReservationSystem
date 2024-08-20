﻿using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Room> Rooms { get; set; }

        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string Status { get; set; }

        public async Task OnGetAsync(int? minPrice, int? maxPrice, string status)
        {

            MinPrice = minPrice;
            MaxPrice = maxPrice;
            Status = status;

            var query = _context.Rooms.Include(r => r.Reviews).AsQueryable();

            // price filter
            if (MinPrice.HasValue)
            {
                query = query.Where(r => r.Price >= MinPrice.Value);
            }

            if (MaxPrice.HasValue)
            {
                query = query.Where(r => r.Price <= MaxPrice.Value);
            }

            // status filter
            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "available")
                {
                    query = query.Where(r => r.IsAvailable);
                }
                else if (Status == "notavailable")
                {
                    query = query.Where(r => !r.IsAvailable);
                }
            }

            Rooms = await query.ToListAsync();
        }
    }
}
