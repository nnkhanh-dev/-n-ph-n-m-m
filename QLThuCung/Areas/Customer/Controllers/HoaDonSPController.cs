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
        private readonly IDanhGiaSPKHService _danhGia;

        public HoaDonSPController(IHoaDonSPKHService hoaDon, UserManager<NguoiDung> userManager, ISanPhamKHService sanPham, IVNPayService vnpayService, IDanhGiaSPKHService danhGia)
        {
            _hoaDon = hoaDon;
            _userManager = userManager;
            _sanPham = sanPham;
            _vnpayService = vnpayService;
            _danhGia = danhGia;
        }

        [Route("khachhang/hoadonsanpham")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.IdNguoiDung = user.Id;
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
                return RedirectToAction("Index", "SanPham");
            }
            if (model.PhuongThucThanhToan == 1)
            {
                model.MaThanhToan = model.NguoiTao + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            }
            model.NgayTao = DateTime.Now;
            var result = await _hoaDon.Create(model);
            if (!result)
            {
                TempData["Error"] = "Đặt mua thất bại!";
                return RedirectToAction("Index", "SanPham");
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

        [Route("khachhang/hoadonsanpham/listbycustomer/{id}")]
        public async Task<IActionResult> ListByCustomer(string id)
        {
            var list = await _hoaDon.ListByCustomer(id);
            return Json(new { Data = list });
        }
        [Route("khachhang/hoadonsanpham/chitiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _hoaDon.Details(id);
            return View(details);
        }

        [Route("khachhang/hoadonsanpham/huy/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _hoaDon.Cancel(id);
            return Json(new { success = result });
        }
        [Route("khachhang/hoadonsanpham/danhgia/{id}")]
        public async Task<IActionResult> Comment(int id)
        {
            var danhGia = new DanhGiaSP();
            danhGia.IdHoaDon = id;
            return View(danhGia);
        }
        [Route("khachhang/hoadonsanpham/danhgia/{idHoaDon}")]
        [HttpPost]
        public async Task<IActionResult> Comment(int idHoaDon, DanhGiaSP model, List<IFormFile> DanhSachAnh)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View(model);
            }
            if (DanhSachAnh != null && DanhSachAnh.Any())
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload");

                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                foreach (var file in DanhSachAnh)
                {
                    if (file != null && file.Length > 0)
                    {
                        var uniqueFileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(uploadPath, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Thêm vào danh sách tệp đính kèm
                        model.TepDinhKem.Add(new TepDinhKemDanhGiaSP
                        {
                            Loai = 1,
                            DuongDan = "/Upload/" + uniqueFileName
                        });
                    }
                }
            }
            var result = await _danhGia.Create(model);
            if (!result)
            {
                TempData["Error"] = "Thêm đánh giá thất bại!";
                return View(model);
            }
            TempData["Success"] = "Thêm đánh giá thành công!";
            return RedirectToAction("Details", new { id = model.IdHoaDon });
        }
    }
}
