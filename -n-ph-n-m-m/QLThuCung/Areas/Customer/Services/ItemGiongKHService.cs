using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;
using System.Collections.Immutable;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemGiongKHService : IGiongKHService
    {
        private readonly AppDbContext _context;

        public ItemGiongKHService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Giong>> List()
        {
            return await _context.Giong.ToListAsync();
        }
    }
}
