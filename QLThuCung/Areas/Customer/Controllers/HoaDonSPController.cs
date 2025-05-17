using HotelApp.Areas.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Customer.Services;
using QLThuCung.Models;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class HoaDonSPController : Controller
    {
        private readonly IHoaDonSPKHService _hoaDon;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly ISanPhamKHService _sanPham;
        private readonly IVNPayService _vnpayService;

        public HoaDonSPController(IHoaDonSPKHService hoaDon, UserManager<NguoiDung> userManager, ISanPhamKHService sanPham, IVNPayService vnpayService)
        {
            _hoaDon = hoaDon;
            _userManager = userManager;
            _sanPham = sanPham;
            _vnpayService = vnpayService;
        }

        [Route("khachhang/hoadonsanpham")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("khachhang/hoadonsanpham/muangay")]
        [HttpPost]
        public async Task<IActionResult> BuyNow([FromBody] List<ChiTietHoaDonSanPham> chiTiets)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                // Create a new HoaDonSanPham
                var hoaDon = new HoaDonSanPham
                {
                    IdKhachHang = user.Id, // Adjust based on your auth system
                    TrangThai = 0, // e.g., 0 for "Pending"
                    PhuongThucThanhToan = 0, // Default payment method, adjust as needed
                    NgayTao = DateTime.Now,
                    NguoiTao = user.Id,
                    ChiTietHoaDonSanPham = new List<ChiTietHoaDonSanPham>(),
                    DanhGia = new List<DanhGiaSP>()
                };

                foreach(var item in chiTiets)
                {
                    var chiTiet = new ChiTietHoaDonSanPham();
                    chiTiet.IdHoaDon = 0;
                    chiTiet.IdSanPham = item.IdSanPham;
                    chiTiet.SoLuong = item.SoLuong;
                    chiTiet.DonGia = item.DonGia;
                    hoaDon.ChiTietHoaDonSanPham.Add(chiTiet);
                }

                // Store HoaDonSanPham in session
                HttpContext.Session.SetString("PendingHoaDon", JsonSerializer.Serialize(hoaDon));

                return Json(new { success = true, message = "Hóa đơn tạm thời đã được tạo!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [Route("khachhang/hoadonsanpham/taomoi")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Lấy hóa đơn từ session
            var hoaDonJson = HttpContext.Session.GetString("PendingHoaDon");
            if (string.IsNullOrEmpty(hoaDonJson))
            {
                return RedirectToAction("Index", "SanPham");
            }
            var hoaDon = JsonSerializer.Deserialize<HoaDonSanPham>(hoaDonJson);
            foreach(var item in hoaDon.ChiTietHoaDonSanPham)
            {
                item.SanPham = await _sanPham.Details(item.IdSanPham);
            }
            return View(hoaDon);
        }
        [Route("khachhang/hoadonsanpham/taomoi")]
        [HttpPost]
        public async Task<IActionResult> Create(HoaDonSanPham model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View(model);
            }
            if (model.PhuongThucThanhToan == 1)
            {
                model.MaThanhToan = model.NguoiTao + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            }
            var result = await _hoaDon.Create(model);
            if (!result)
            {
                TempData["Error"] = "Đặt mua thất bại!";
                return View(model);
            }
            HttpContext.Session.Remove("PendingHoaDon");
            if (model.PhuongThucThanhToan == 1)
            {
                decimal total = 0;
                foreach(var item in model.ChiTietHoaDonSanPham)
                {
                    total += item.DonGia * item.SoLuong;
                }
                string note = model.MaThanhToan;
                return Redirect(_vnpayService.CreatePaymentUrl(HttpContext, total, note));
            }
            else
            {
                TempData["Success"] = "Đặt mua thành công!";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
