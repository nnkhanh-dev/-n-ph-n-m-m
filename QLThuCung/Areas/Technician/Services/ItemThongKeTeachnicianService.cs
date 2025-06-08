using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Technician.ViewModels;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Technician.Services
{
    public class ItemThongKeTechnicianService : IThongKeTechnicianService
    {
        private readonly AppDbContext _context;
        public ItemThongKeTechnicianService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DoanhThuVM>> DoanhThu()
        {
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
            var list = await _context.ChiTietHoaDonSanPham.Include(x => x.SanPham)
                                                          .Include(x => x.HoaDon)
                                                          .Where(x => x.HoaDon.TrangThai == 3)
                                                          .ToListAsync();

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
            var list = await _context.ChiTietHoaDonDichVu.Include(x => x.DichVu)
                                                         .Include(x => x.HoaDon)
                                                         .Where(x => x.HoaDon.TrangThai == 3)
                                                         .ToListAsync();

            var result = list
                .GroupBy(x => x.IdDichVu)
                .Select(group => new DoanhThuDVVM
                {
                    Id = group.Key,
                    TenDichVu = group.First().DichVu?.Ten,
                    DoanhThu = group.Sum(x => x.DonGia)
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

        public async Task<byte[]> XuatExcel()
        {
            var data = await DoanhThu(); // lấy từ phương thức bạn đã viết

            // Group dữ liệu theo năm
            var groupByYear = data
                .GroupBy(d => d.Nam)
                .OrderBy(g => g.Key); // để thứ tự năm tăng dần

            using (var workbook = new XLWorkbook())
            {
                foreach (var yearGroup in groupByYear)
                {
                    var worksheet = workbook.Worksheets.Add($"Năm {yearGroup.Key}");

                    // Tiêu đề đầu
                    worksheet.Cell("A1").Value = $"Doanh thu năm {yearGroup.Key}";
                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Font.FontSize = 16;

                    // Header
                    worksheet.Cell("A3").Value = "Tháng";
                    worksheet.Cell("B3").Value = "Quý";
                    worksheet.Cell("C3").Value = "Doanh thu từ sản phẩm";
                    worksheet.Cell("D3").Value = "Doanh thu từ dịch vụ";
                    worksheet.Cell("E3").Value = "Tổng doanh thu";

                    worksheet.Range("A3:E3").Style.Font.Bold = true;
                    worksheet.Range("A3:E3").Style.Fill.BackgroundColor = XLColor.LightGray;

                    // Group theo Tháng và Quý trong cùng năm
                    var monthlyData = yearGroup
                        .GroupBy(x => new { x.Thang, x.Quy })
                        .OrderBy(x => x.Key.Thang);

                    int row = 4;
                    foreach (var group in monthlyData)
                    {
                        var thang = group.Key.Thang;
                        var quy = group.Key.Quy;

                        var dv = group.Where(x => x.Loai == "Dịch vụ").Sum(x => x.DoanhThu);
                        var sp = group.Where(x => x.Loai == "Sản phẩm").Sum(x => x.DoanhThu);
                        var tong = dv + sp;

                        worksheet.Cell(row, 1).Value = thang;
                        worksheet.Cell(row, 2).Value = quy;
                        worksheet.Cell(row, 3).Value = sp;
                        worksheet.Cell(row, 4).Value = dv;
                        worksheet.Cell(row, 5).Value = tong;

                        row++;
                    }

                    // Sau khi viết dữ liệu xong
                    worksheet.Column(1).Width = 10;  // Tháng
                    worksheet.Column(2).Width = 10;  // Quý
                    worksheet.Column(3).Width = 25;  // SP
                    worksheet.Column(4).Width = 25;  // DV
                    worksheet.Column(5).Width = 20;  // Tổng

                    worksheet.Range($"A4:A{row - 1}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Range($"B4:B{row - 1}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Range($"C4:E{row - 1}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                }

                // Xuất thành mảng byte
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

    }

}
