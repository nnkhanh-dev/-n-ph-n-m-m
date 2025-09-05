using System.Diagnostics;
using System.Threading.Tasks;
using HotelApp.Areas.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Customer.Models;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVNPayService _vnpayService;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, IVNPayService vnpayService, AppDbContext context)
        {
            _logger = logger;
            _vnpayService = vnpayService;
            _context = context;
        }
        [Route("/khach-hang")]
        public async Task<IActionResult> Index()
        {
            if (Request.Query.Keys.Any(k => k.StartsWith("vnp_")))
            {
                var response = _vnpayService.PaymentExecute(Request.Query);
                if (response == null || response.VnPayResponseCode != "00")
                {
                    TempData["Error"] = "Thanh toán thất bại: Mã phản hồi từ VNPay không hợp lệ.";
                    return View();
                }

                var code = response.OrderDescription;
                var hoaDonDichVu = await _context.HoaDonDichVu
                    .FirstOrDefaultAsync(x => x.MaThanhToan == code);
                var hoaDonSanPham = await _context.HoaDonSanPham
                    .Include(x => x.ChiTietHoaDonSanPham)
                    .ThenInclude(x => x.SanPham)
                    .FirstOrDefaultAsync(x => x.MaThanhToan == code);

                if (hoaDonDichVu == null && hoaDonSanPham == null)
                {
                    TempData["Error"] = "Thanh toán thất bại: Không tìm thấy hóa đơn tương ứng.";
                    return View();
                }

                try
                {
                    if (hoaDonDichVu != null)
                    {
                        hoaDonDichVu.TrangThai = 0; // Cập nhật trạng thái hóa đơn dịch vụ
                        _context.HoaDonDichVu.Update(hoaDonDichVu);
                    }

                    if (hoaDonSanPham != null)
                    {
                        hoaDonSanPham.TrangThai = 0; // Cập nhật trạng thái hóa đơn sản phẩm
                        _context.HoaDonSanPham.Update(hoaDonSanPham);

                        foreach (var item in hoaDonSanPham.ChiTietHoaDonSanPham)
                        {
                            if (item.SanPham.SoLuong < item.SoLuong)
                            {
                                TempData["Error"] = "Thanh toán thất bại: Số lượng sản phẩm không đủ.";
                                return View();
                            }
                            item.SanPham.SoLuong -= item.SoLuong;
                            _context.SanPham.Update(item.SanPham);
                        }
                    }

                    // Lưu tất cả thay đổi trong một giao dịch
                    var result = await _context.SaveChangesAsync();
                    if (result <= 0)
                    {
                        TempData["Error"] = "Thanh toán thất bại: Không thể cập nhật cơ sở dữ liệu.";
                        return View();
                    }

                    TempData["Success"] = "Thanh toán thành công.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Thanh toán thất bại: Lỗi hệ thống - {ex.Message}";
                    return View();
                }
            }

            // Trường hợp không phải callback từ VNPay
            return View();
        }
        [Route("/khachhang/chungtoi")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("/khachhang/lienhe")]
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModelCustomer { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
