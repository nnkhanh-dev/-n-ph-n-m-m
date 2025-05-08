using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Customer.Services;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class HoaDonDVController : Controller
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IHoaDonDVKHService _hoaDon;

        public HoaDonDVController(UserManager<NguoiDung> userManager, IHoaDonDVKHService hoaDon)
        {
            _userManager = userManager;
            _hoaDon = hoaDon;
        }
        [Route("khachhang/hoadondichvu")]
        public IActionResult Index()
        {
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
                TempData["error"] = "Dữ liệu không hợp lệ!";
                return View();
            }

            var result = await _hoaDon.Create(model);

            if (!result)
            {
                TempData["Error"] = "Đặt lịch thất bại!";
                return View();
            }

            TempData["Success"] = "Đặt lịch thành công!";
            return RedirectToAction("Index", "DichVu");
        }

        [Route("khachhang/datlich/listbydate/{ngayChamSoc}")]
        public async Task<IActionResult> ListByDate(DateTime ngayChamSoc)
        {
            var list = await _hoaDon.ListByDate(ngayChamSoc);
            return Json(new {Data = list});
        }

    }
}
