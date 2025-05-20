using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Admin.Models;
using QLThuCung.Areas.Admin.Services;
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
        private readonly IThongKeService _thongKe;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<NguoiDung> userManager, IThongKeService thongKe)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _thongKe = thongKe;
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

            ViewBag.TopSanPham = await _thongKe.TopSanPham();
            ViewBag.TopDichVu = await _thongKe.TopDichVu();

            return View();
        }
        [Route("/admin/doanhthu")]
        public async Task<IActionResult> DoanhThu()
        {
            var doanhThu = await _thongKe.DoanhThu();
            return Json(new { Data = doanhThu });
        }
        [Route("/admin/doanhthu/sanpham")]
        public async Task<IActionResult> DoanhThuSanPham()
        {
            var result = await _thongKe.DoanhThuSanPham();
            return Json(new {Data = result});
        }

        [Route("/admin/doanhthu/dichvu")]
        public async Task<IActionResult> DoanhThuDichVu()
        {
            var result = await _thongKe.DoanhThuDichVu();
            return Json(new { Data = result });
        }

        [Route("/admin/doanhthu/xuatexcel")]
        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                var excelBytes = await _thongKe.XuatExcel();

                if (excelBytes == null || excelBytes.Length == 0)
                {
                    TempData["Error"] = "Không có dữ liệu để xuất.";
                    return RedirectToAction("Index"); // hoặc trang phù hợp
                }
                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "DanhSach.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi xuất file Excel: " + ex.Message;
                return RedirectToAction("Index"); // hoặc trang phù hợp
            }
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
