using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Technician.Services;
using QLThuCung.Data;
using QLThuCung.Models;
using QLThuCung.Technician.Models;

namespace QLThuCung.Technician.Controllers
{
    [Area("Technician")]
    [Authorize(Roles = "KyThuatVien")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IThongKeTechnicianService _thongKe;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<NguoiDung> userManager, IThongKeTechnicianService thongKe)
        {
            _logger = logger;
            _thongKe = thongKe;
            _context = context;
            _userManager = userManager;
        }
        [Route("/technician")]
        public async Task<IActionResult> Index()
        {
            ViewBag.SoLuongSanPham = await _context.SanPham.CountAsync();

            // L?y s? l??ng d?ch v?
            ViewBag.SoLuongDichVu = await _context.DichVu.CountAsync();

            var nhanVienList = await _userManager.GetUsersInRoleAsync("NhanVien");
            ViewBag.SoLuongNhanVien = nhanVienList.Count;

            // L?y s? l??ng k? thu?t viên
            var kyThuatVienList = await _userManager.GetUsersInRoleAsync("KyThuatVien");
            ViewBag.SoLuongKyThuatVien = kyThuatVienList.Count;

            ViewBag.TopSanPham = await _thongKe.TopSanPham();
            ViewBag.TopDichVu = await _thongKe.TopDichVu();

            return View();
        }

        [Route("/technician/doanhthu")]
        public async Task<IActionResult> DoanhThu()
        {
            var doanhThu = await _thongKe.DoanhThu();
            return Json(new { Data = doanhThu });
        }
        [Route("/technician/doanhthu/sanpham")]
        public async Task<IActionResult> DoanhThuSanPham()
        {
            var result = await _thongKe.DoanhThuSanPham();
            return Json(new { Data = result });
        }

        [Route("/technician/doanhthu/dichvu")]
        public async Task<IActionResult> DoanhThuDichVu()
        {
            var result = await _thongKe.DoanhThuDichVu();
            return Json(new { Data = result });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModelTechnician { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
