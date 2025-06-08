using HotelApp.Areas.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.Models;
using QLThuCung.ViewModels;
using System.Security.Claims;

namespace QLThuCung.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "NhanVien")]
    public class HoaDonSPController1 : Controller
    {
        private readonly IHoaDonSanPhamAdminService _hoadonsp;
        private readonly ISanphamService _sanpham;
        private readonly IHoaDonSanPhamAdminService _hoadon;
        private readonly IVNPayService _vnpayService;
        private readonly ItemNguoiDungAdminService _nguoidung;

        public HoaDonSPController1(IHoaDonSanPhamAdminService hoadonsp, ISanphamService sanpham, IHoaDonSanPhamAdminService hoadon, IVNPayService vNPayService, ItemNguoiDungAdminService nguoidung)
        {
            _hoadonsp = hoadonsp;
            _sanpham = sanpham;
            _hoadon = hoadon;
            _vnpayService = vNPayService;
            _nguoidung = nguoidung;
        }

        [Route("employee/hoadonsanpham")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("employee/hoadonsanpham/list")]
        public async Task<IActionResult> List()
        {
            var list = await _hoadonsp.List();
            return Json(new { Data = list });
        }

        [Route("employee/hoadonsanpham/chitiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _hoadonsp.Details(id);
            return View(details);
        }

        [Route("employee/hoadonsanpham/buy")]
        public async Task<IActionResult> Buy()
        {
            var register = new RegisterVM();
            var list = await _sanpham.List();
            ViewBag.SanPham = list;
            var nguoiDungs = await _nguoidung.List();
            ViewBag.NguoiDung = nguoiDungs.Select(dm => new SelectListItem
            {
                Value = dm.Id.ToString(),
                Text = dm.PhoneNumber,
            }).ToList();
            return View(register);
        }

        [HttpPost]
        [Route("employee/hoadonsanpham/buy")]
        public async Task<IActionResult> Buy(List<ChiTietHoaDonSanPham> ChiTietHoaDonSanPham, string customerPhone, string Method)
        {
            var model = new HoaDonSanPham();
            var user = await _nguoidung.Details(customerPhone);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.IdKhachHang = user.Id;
            model.PhuongThucThanhToan = 0;
            model.NguoiTao = userId;
            model.DiaChi = "Địa chỉ của store";
            model.ChiTietHoaDonSanPham = ChiTietHoaDonSanPham;
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                var register = new RegisterVM();
                var list = await _sanpham.List();
                ViewBag.SanPham = list;
                var nguoiDungs = await _nguoidung.List();
                ViewBag.NguoiDung = nguoiDungs.Select(dm => new SelectListItem
                {
                    Value = dm.Id.ToString(),
                    Text = dm.PhoneNumber,
                }).ToList();
                return View(register);
            }
            model.MaThanhToan = model.NguoiTao + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

            model.NgayTao = DateTime.Now;
            var result = await _hoadon.Create(model);
            if (!result)
            {
                TempData["Error"] = "Đặt mua thất bại!";
                return RedirectToAction("Index", "SanPham");
            }
            HttpContext.Session.Remove("PendingHoaDon");
            if (model.PhuongThucThanhToan == 1 && Method == "Cash")
            {
                string note = model.MaThanhToan;
                await _hoadonsp.UpdateStatus(note);
                TempData["Success"] = "Đặt mua thành công!";
                return RedirectToAction("Index");
            }
            if (model.PhuongThucThanhToan == 1)
            {
                decimal total = 0;
                foreach (var item in model.ChiTietHoaDonSanPham)
                {
                    total += item.DonGia * item.SoLuong;
                }
                string note = model.MaThanhToan;
                return Redirect(_vnpayService.CreatePaymentUrl(HttpContext, total, note));
            }
            TempData["Error"] = "Đặt mua thất bại!";
            return RedirectToAction("Index", "SanPham");
        }

        [Route("/employee/hoadonsanpham/update")]
        [HttpGet]
        public async Task<IActionResult> Update(string id, string status)
        {
            var result = false;
            if (status == "3")
            {
                result = await _hoadonsp.UpdateSuccessStatus(id);
            }
            else
            {
                result = await _hoadonsp.UpdateByStatus(id, status);
            }
            return Json(new { success = result });
        }
    }
}
