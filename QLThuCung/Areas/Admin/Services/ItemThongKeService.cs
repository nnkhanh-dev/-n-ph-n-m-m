using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Data;
using QLThuCung.Models;

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

        public async Task<IEnumerable<DoanhThuSPVM>> DoanhThuSanPham()
        {
            var list = await _context.ChiTietHoaDonSanPham.Include(x => x.SanPham).ToListAsync();

            var result = list
                .GroupBy(x => x.IdSanPham)
                .Select(group => new DoanhThuSPVM
                {
                    Id = group.Key,
                    TenSanPham = group.First().SanPham?.Ten, 
                    DoanhThu = group.Sum(x => x.SoLuong * x.DonGia)
                }).ToList();

            return result;
        }

        public async Task<IEnumerable<DoanhThuDVVM>> DoanhThuDichVu()
        {
            var list = await _context.ChiTietHoaDonDichVu.Include(x => x.DichVu).ToListAsync();

            var result = list
                .GroupBy(x => x.IdDichVu)
                .Select(group => new DoanhThuDVVM
                {
                    Id = group.Key,
                    TenDichVu = group.First().DichVu?.Ten,
                    DoanhThu = group.Sum(x =>x.DonGia)
                }).ToList();

            return result;
        }

        public async Task<IEnumerable<DichVu>> TopDichVu()
        {
            var list = await _context.ChiTietHoaDonDichVu
                .Include(ct => ct.DichVu)
                .ToListAsync(); // Lấy toàn bộ dữ liệu trước

            var DichVu = list
                .GroupBy(ct => ct.IdDichVu)
                .Select(g => new
                {
                    DichVu = g.First().DichVu,
                    DoanhThu = g.Sum(ct => ct.DonGia)
                })
                .OrderByDescending(x => x.DoanhThu)
                .Select(x => x.DichVu)
                .ToList();

            return DichVu;
        }

        public async Task<IEnumerable<SanPham>> TopSanPham()
        {
            var list = await _context.ChiTietHoaDonSanPham
                .Include(ct => ct.SanPham)
                .ToListAsync(); // Lấy toàn bộ dữ liệu trước

            var SanPham = list
                .GroupBy(ct => ct.IdSanPham)
                .Select(g => new
                {
                    SanPham = g.First().SanPham,
                    DoanhThu = g.Sum(ct => ct.DonGia * ct.SoLuong)
                })
                .OrderByDescending(x => x.DoanhThu)
                .Select(x => x.SanPham)
                .ToList();

            return SanPham;
        }
    }

}
