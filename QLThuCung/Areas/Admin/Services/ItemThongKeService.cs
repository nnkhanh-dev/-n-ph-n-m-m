using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Data;

namespace QLThuCung.Areas.Admin.Services
{
    public class ItemThongKeService : IThongKeService
    {
        private readonly AppDbContext _context;

        public ItemThongKeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoanhThuVM>> DoanhThu()
        {
            // Fetch and process HoaDonDichVu
            var hoaDonDichVus = await _context.HoaDonDichVu
                .Where(x => x.TrangThai == 3)
                .Include(x => x.ChiTietHoaDonDichVu)
                .ToListAsync();

            var doanhThuDichVuList = hoaDonDichVus
                .GroupBy(hd => new
                {
                    Thang = hd.NgayTao.Month,
                    Quy = (hd.NgayTao.Month - 1) / 3 + 1,
                    Nam = hd.NgayTao.Year
                })
                .Select(g => new DoanhThuVM
                {
                    Thang = g.Key.Thang,
                    Quy = g.Key.Quy,
                    Nam = g.Key.Nam,
                    Loai = "Dịch vụ",
                    DoanhThu = g.Sum(hd => hd.ChiTietHoaDonDichVu.Sum(ct => ct.DonGia)) 
                });

            // Fetch and process HoaDonSanPham
            var hoaDonSanPhams = await _context.HoaDonSanPham
                .Where(x => x.TrangThai == 3)
                .Include(x => x.ChiTietHoaDonSanPham)
                .ToListAsync();

            var doanhThuSanPhamList = hoaDonSanPhams
                .GroupBy(hd => new
                {
                    Thang = hd.NgayTao.Month,
                    Quy = (hd.NgayTao.Month - 1) / 3 + 1,
                    Nam = hd.NgayTao.Year
                })
                .Select(g => new DoanhThuVM
                {
                    Thang = g.Key.Thang,
                    Quy = g.Key.Quy,
                    Nam = g.Key.Nam,
                    Loai = "Sản phẩm",
                    DoanhThu = g.Sum(hd => hd.ChiTietHoaDonSanPham.Sum(ct => ct.DonGia * ct.SoLuong))
                });

            // Combine both lists
            var combinedList = doanhThuDichVuList
                .Concat(doanhThuSanPhamList)
                .ToList();

            return combinedList;
        }
    }
            
}
