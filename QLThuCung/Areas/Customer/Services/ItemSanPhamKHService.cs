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
            var item = await _context.SanPham
                .Include(x => x.AnhSanPham)
                .Include(x => x.DanhMuc)
                .Include(sp => sp.ChiTietHoaDonSanPham)
                    .ThenInclude(ct => ct.HoaDon)
                        .ThenInclude(hd => hd.KhachHang)

                .Include(sp => sp.ChiTietHoaDonSanPham)
                    .ThenInclude(ct => ct.HoaDon)
                        .ThenInclude(hd => hd.DanhGia)
                            .ThenInclude(tdk => tdk.TepDinhKem)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return null;
            }

            // Lọc ChiTietHoaDonSanPham theo IdSanPham
            item.ChiTietHoaDonSanPham = item.ChiTietHoaDonSanPham
                .Where(ct => ct.IdSanPham == id)
                .ToList();

            // Kiểm tra và log để debug
            foreach (var ct in item.ChiTietHoaDonSanPham)
            {
                if (ct.HoaDon?.KhachHang == null)
                {
                    Console.WriteLine($"HoaDon {ct.HoaDon?.Id} không có NguoiDung cho IdSanPham {id}");
                }
                foreach (var dg in ct.HoaDon?.DanhGia ?? new List<DanhGiaSP>())
                {
                    Console.WriteLine($"DanhGia Id={dg.Id}, NoiDung={dg.NoiDung}, IdHoaDon={dg.IdHoaDon}, KhachHangId={ct.HoaDon?.KhachHang?.Id}");
                }
            }

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
