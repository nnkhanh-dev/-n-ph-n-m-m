using HotelApp.Areas.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Customer.Services;
using QLThuCung.Models;
using System.Threading.Tasks;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class HoaDonDVController : Controller
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IHoaDonDVKHService _hoaDon;
        private readonly IVNPayService _vnpayService;
        private readonly IDanhGiaDVKHService _danhGia;

        public HoaDonDVController(UserManager<NguoiDung> userManager, IHoaDonDVKHService hoaDon, IVNPayService vnpayService, IDanhGiaDVKHService danhGia)
        {
            _userManager = userManager;
            _hoaDon = hoaDon;
            _vnpayService = vnpayService;
            _danhGia = danhGia;
        }
        [Route("khachhang/hoadondichvu")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.IdNguoiDung = user.Id;
            return View();
        }

        [Route("khachhang/datlich/")]
        public async Task<IActionResult> Booking()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.UserId = userId;
            return View();
        }
        [Route("khachhang/datlich/")]
        [HttpPost]
        public async Task<IActionResult> Booking(HoaDonDichVu model)
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.UserId = userId;
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View();
            }
            if (model.PhuongThucThanhToan == 1)
            {
                model.MaThanhToan = model.NguoiTao + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            }
            var result = await _hoaDon.Create(model);

            if (!result)
            {
                TempData["Error"] = "Đặt lịch thất bại!";
                return View();
            }

            if (model.PhuongThucThanhToan == 1)
            {
                decimal total = await _hoaDon.TotalPrice(model);
                string note = model.MaThanhToan;
                return Redirect(_vnpayService.CreatePaymentUrl(HttpContext, total, note));
            }
            else
            {
                TempData["Success"] = "Đặt lịch thành công!";
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("khachhang/datlich/listbydate/{ngayChamSoc}")]
        public async Task<IActionResult> ListByDate(DateTime ngayChamSoc)
        {
            var list = await _hoaDon.ListByDate(ngayChamSoc);
            return Json(new { Data = list });
        }
        [Route("khachhang/datlich/listbycustomer/{id}")]
        public async Task<IActionResult> ListByCustomer(string id)
        {
            var list = await _hoaDon.ListByCustomer(id);
            return Json(new { Data = list });
        }
        [Route("khachhang/hoadondichvu/chitiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _hoaDon.Details(id);
            return View(details);
        }

        [Route("khachhang/datlich/huy/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _hoaDon.Cancel(id);
            return Json(new { success = result });
        }
        [Route("khachhang/hoadondichvu/danhgia/{id}")]
        public async Task<IActionResult> Comment(int id)
        {
            var danhGia = new DanhGiaDV();
            danhGia.IdHoaDon = id;
            return View(danhGia);
        }
        [Route("khachhang/hoadondichvu/danhgia/{idHoaDon}")]
        [HttpPost]
        public async Task<IActionResult> Comment(int idHoaDon, DanhGiaDV model, List<IFormFile> DanhSachAnh)
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
                        model.TepDinhKem.Add(new TepDinhKemDanhGiaDV
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

        [Route("khachhang/hoadondichvu/danhgia/huy/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _danhGia.Delete(id);
            return Json(new { success = result });
        }
    }
}
