using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Admin.Models;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<NguoiDung> _userManager;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<NguoiDung> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        [Route("/admin/")]
        public async Task<IActionResult> Index()
        {
            // L?y s? l??ng s?n ph?m
            ViewBag.SoLuongSanPham = await _context.SanPham.CountAsync();

            // L?y s? l??ng d?ch v?
            ViewBag.SoLuongDichVu = await _context.DichVu.CountAsync();

            var nhanVienList = await _userManager.GetUsersInRoleAsync("NhanVien");
            ViewBag.SoLuongNhanVien = nhanVienList.Count;

            // L?y s? l??ng k? thu?t viên
            var kyThuatVienList = await _userManager.GetUsersInRoleAsync("KyThuatVien");
            ViewBag.SoLuongKyThuatVien = kyThuatVienList.Count;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModelAdmin { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
