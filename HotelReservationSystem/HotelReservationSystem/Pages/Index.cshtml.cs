using HotelReservationSystem.Data;
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

        // Search parameters
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }  // Nullable DateTime
        public DateTime? ToDate { get; set; }    // Nullable DateTime

        public List<Room> Rooms { get; set; }

        public void OnGet(int? minPrice, int? maxPrice, string status, DateTime? fromDate, DateTime? toDate)
        {
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            Status = status;
            FromDate = fromDate;
            ToDate = toDate;

            var query = _context.Rooms.AsQueryable();

            if (MinPrice.HasValue)
            {
                query = query.Where(r => r.Price >= MinPrice.Value);
            }

            if (MaxPrice.HasValue)
            {
                query = query.Where(r => r.Price <= MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(Status))
            {
                bool isAvailable = Status == "available";
                query = query.Where(r => r.IsAvailable == isAvailable);
            }

            if (FromDate.HasValue)
            {
                query = query.Where(r => r.FromDate >= FromDate.Value);
            }

            if (ToDate.HasValue)
            {
                query = query.Where(r => r.ToDate <= ToDate.Value);
            }

            Rooms = query.Include(r => r.Reviews).ToList();
        }
    }
}
