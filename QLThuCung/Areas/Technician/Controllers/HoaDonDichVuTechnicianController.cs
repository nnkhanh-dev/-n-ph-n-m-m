using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.Areas.Technician.Services;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Technician.Controllers
{
    [Area("Technician")]
    [Authorize(Roles = "KyThuatVien")]

    public class HoaDonDichVuTechnicianController : Controller
    {
        private readonly IHoaDonDichVuTechnicianService _hoaDonDichVuService;
        private readonly AppDbContext _context;
        public HoaDonDichVuTechnicianController(IHoaDonDichVuTechnicianService hoaDonDichVuService, AppDbContext context)
        {
            _context = context;
            _hoaDonDichVuService = hoaDonDichVuService;
        }

        [Route("technician/hoadondichvutechnician")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("technician/hoadondichvutechnician/list")]
        public async Task<IActionResult> List()
        {
            var list = await _hoaDonDichVuService.List();

            var data = new List<object>();

            foreach (var h in list)
            {
                var tongTien = await _hoaDonDichVuService.TotalPrice(h);

                data.Add(new
                {
                    id = h.Id,
                    maHoaDon = h.Id,
                    idKhachHang = h.ThuCung?.IdKhachHang ?? "Chưa xác định",
                    tenKhachHang = h.ThuCung?.KhachHang?.HoTen ?? "Chưa xác định",
                    tenThuCung = h.ThuCung?.Ten ?? "Chưa có",
                    ngayChamSoc = h.NgayChamSoc,
                    trangThai = h.TrangThai,
                    tongTien = tongTien
                });
            }

            return Json(new { data });
        }


        [HttpGet]
        [Route("technician/hoadondichvutechnician/listpendingtoday")]
        public async Task<IActionResult> ListPendingToday()
        {
            var today = DateTime.Today; // Ngày hiện tại: 02/06/2025
            //today = DateTime.Parse("05/23/2025"); // MM//DD/YYYY
            var list = await _hoaDonDichVuService.List();
            var pendingTodayList = list.Where(h => h.NgayChamSoc.Date == today).ToList();

            var data = new List<object>();

            foreach (var h in pendingTodayList)
            {
                var tongTien = await _hoaDonDichVuService.TotalPrice(h);

                data.Add(new
                {
                    id = h.Id,
                    maHoaDon = h.Id,
                    idKhachHang = h.ThuCung?.IdKhachHang ?? "Chưa xác định",
                    tenKhachHang = h.ThuCung?.KhachHang?.HoTen ?? "Chưa xác định",
                    tenThuCung = h.ThuCung?.Ten ?? "Chưa có",
                    ngayChamSoc = h.NgayChamSoc,
                    trangThai = h.TrangThai,
                    tongTien = tongTien
                });
            }

            return Json(new { data });
        }

        [HttpGet]
        [Route("technician/hoadondichvutechnician/filterbydate")]
        public async Task<IActionResult> FilterByDate(DateTime? filterDate)
        {
            var list = await _hoaDonDichVuService.List();
            if (filterDate.HasValue)
            {
                list = list.Where(h => h.NgayChamSoc.Date == filterDate.Value.Date).ToList();
            }

            var data = new List<object>();
            foreach (var h in list)
            {
                var tongTien = await _hoaDonDichVuService.TotalPrice(h);
                data.Add(new
                {
                    id = h.Id,
                    maHoaDon = h.Id,
                    idKhachHang = h.ThuCung?.IdKhachHang ?? "Chưa xác định",
                    tenKhachHang = h.ThuCung?.KhachHang?.HoTen ?? "Chưa xác định",
                    tenThuCung = h.ThuCung?.Ten ?? "Chưa có",
                    ngayChamSoc = h.NgayChamSoc,
                    tongTien = tongTien,
                    trangThai = h.TrangThai
                });
            }
            return Json(new { data });
        }

        [HttpGet]
        [Route("technician/hoadondichvutechnician/filterbystatus")]
        public async Task<IActionResult> FilterByStatus(int? trangThai)
        {
            var list = await _hoaDonDichVuService.List();
            if (trangThai.HasValue)
            {
                list = list.Where(h => h.TrangThai == trangThai.Value).ToList();
            }

            var data = new List<object>();
            foreach (var h in list)
            {
                var tongTien = await _hoaDonDichVuService.TotalPrice(h);
                data.Add(new
                {
                    id = h.Id,
                    maHoaDon = h.Id,
                    idKhachHang = h.ThuCung?.IdKhachHang ?? "Chưa xác định",
                    tenKhachHang = h.ThuCung?.KhachHang?.HoTen ?? "Chưa xác định",
                    tenThuCung = h.ThuCung?.Ten ?? "Chưa có",
                    ngayChamSoc = h.NgayChamSoc,
                    tongTien = tongTien,
                    trangThai = h.TrangThai
                });
            }
            return Json(new { data });
        }

        [HttpGet]
        [Route("technician/hoadondichvutechnician/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _hoaDonDichVuService.Details(id);
            if (details == null)
            {
                return NotFound();
            }

            ViewBag.Giuongs = _context.Giuong.ToList(); 
            ViewBag.SelectedGiuongId = details.IdGiuong;

            var occupiedGiuongIds = await _context.HoaDonDichVu
                .Where(h => h.Id != id &&
                            h.NgayChamSoc == details.NgayChamSoc &&
                            h.ThoiGianChamSoc == details.ThoiGianChamSoc &&
                            h.IdGiuong != null)
                .Select(h => h.IdGiuong.Value)
                .Distinct()
                .ToListAsync();

            ViewBag.OccupiedGiuongIds = occupiedGiuongIds;
            return View(details);
        }

        [HttpPost]
        [Route("technician/hoadondichvutechnician/updategiuong/{id}")]
        public async Task<IActionResult> UpdateGiuong(int id, int idGiuong)
        {
            var hoaDon = await _hoaDonDichVuService.Details(id);
            if (hoaDon == null)
            {
                return Json(new { success = false });
            }

            hoaDon.IdGiuong = idGiuong;

            var chiTietHoaDonDichVuList = hoaDon.ChiTietHoaDonDichVu?.ToList() ?? new List<ChiTietHoaDonDichVu>();
            var result = await _hoaDonDichVuService.Edit(id, hoaDon, chiTietHoaDonDichVuList, hoaDon.BatDau?.ToString("HH:mm"), hoaDon.KetThuc?.ToString("HH:mm"));

            return Json(new { success = result });
        }
    }
}
