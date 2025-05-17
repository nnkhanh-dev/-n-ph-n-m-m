using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemDanhMucService : IDanhMucService
    {
        private readonly AppDbContext _context;

        public ItemDanhMucService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DanhMuc>> List()
        {
            var list = await _context.DanhMuc.ToListAsync();
            return list;
        }
    }
}
