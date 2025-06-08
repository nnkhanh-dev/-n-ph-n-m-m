using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemSanPhamKHService : ISanPhamKHService
    {
        private readonly AppDbContext _context;

        public ItemSanPhamKHService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SanPham> Details(int id)
        {
            var item = await _context.SanPham.Include(x => x.AnhSanPham)
                                             .Include(x => x.DanhMuc)
                                             .Include(x => x.ChiTietHoaDonSanPham)
                                                .ThenInclude(x => x.HoaDon)
                                                    .ThenInclude(x => x.DanhGia)
                                             .FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<IEnumerable<SanPham>> List()
        {
            var list = await _context.SanPham.Include(x => x.AnhSanPham)
                                             .Include(x => x.DanhMuc)
                                             .Include(x => x.ChiTietHoaDonSanPham)
                                                .ThenInclude(x => x.HoaDon)
                                                    .ThenInclude(x => x.DanhGia)
                                             .ToListAsync();
            return list;
        }
    }
}
