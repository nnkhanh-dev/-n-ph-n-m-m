using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemLoaiKHService : ILoaiKHService
    {
        private readonly AppDbContext _context;

        public ItemLoaiKHService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loai>> List()
        {
            return await _context.Loai.Include(x => x.Giong)
                                      .ToListAsync();
        }
    }
}
