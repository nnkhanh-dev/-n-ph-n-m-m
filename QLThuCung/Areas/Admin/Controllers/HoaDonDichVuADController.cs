using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.Models;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using Microsoft.AspNetCore.Identity;
using HotelApp.Areas.Client.Services;
using QLThuCung.ViewModels;
using QLThuCung.Areas.Customer.Services;

namespace QLThuCung.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HoaDonDichVuADController : Controller
    {
        private readonly IHoaDonDichVuADService _hoaDonDichVuADService;
        private readonly AppDbContext _context;
        private readonly IVNPayService _vnpayService;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IThuCungKHService _thuCung;

        private static HoaDonDichVu currentHDDV = new HoaDonDichVu();

        public HoaDonDichVuADController(IHoaDonDichVuADService hoaDonDichVuADService, AppDbContext context, IVNPayService vNPayService, UserManager<NguoiDung> userManager, IThuCungKHService thuCung)
        {
            _hoaDonDichVuADService = hoaDonDichVuADService;
            _context = context;
            _vnpayService = vNPayService;
            _userManager = userManager;
            _thuCung = thuCung;
        }

        [Route("admin/hoadondichvuad")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("admin/hoadondichvuad/list")]
        public async Task<IActionResult> List()
        {
            var list = await _hoaDonDichVuADService.List();

            var data = new List<object>();

            foreach (var h in list)
            {
                var tongTien = await _hoaDonDichVuADService.TotalPrice(h);

                data.Add(new
                {
                    id = h.Id,
                    maHoaDon = h.Id,
                    idKhachHang = h.ThuCung?.IdKhachHang ?? "Chưa xác định",
                    tenKhachHang = h.ThuCung?.KhachHang?.HoTen ?? "Chưa xác định",
                    tenThuCung = h.ThuCung?.Ten ?? "Chưa có",
                    ngayChamSoc = h.NgayChamSoc,
                    tongTien = tongTien
                });
            }

            return Json(new { data });
        }

        [HttpDelete]
        [Route("admin/hoadondichvuad/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _hoaDonDichVuADService.Delete(id);
                return Json(new { success = result, message = result ? "Xóa hóa đơn thành công!" : "Xóa hóa đơn thất bại!" });
            }
            catch (KeyNotFoundException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("admin/hoadondichvuad/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("admin/hoadondichvuad/create")]
        public async Task<IActionResult> Create(HoaDonDichVu model)
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.UserId = userId;
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ! Vui lòng kiểm tra lại các trường bắt buộc.";
                return View(model);
            }
            model.NguoiTao = userId;
            
            var result = await _hoaDonDichVuADService.Create(model);
            TempData["Success"] = "Tạo mới thành công!";
            return RedirectToAction("Index", "HoaDonDichVuAD");
        }

        [HttpGet]
        [Route("admin/hoadondichvuad/searchkhachhang")]
        public IActionResult SearchKhachHang(string term)
        {
            var khachHangs = _context.Users
                .Where(u => u.PhoneNumber.Contains(term)) // Tìm kiếm theo số điện thoại
                .Select(u => new { id = u.Id, hoTen = u.HoTen, phoneNumber = u.PhoneNumber }) // Trả về thêm PhoneNumber
                .Take(10)
                .ToList();
            return Json(khachHangs);
        }

        [Route("admin/thucung/list/{id}")]
        public async Task<IActionResult> List(string id)
        {
            var list = await _thuCung.List(id);
            return Json(new { Data = list });
        }

        [Route("admin/hoadondichvuad/getdichvu")]
        [HttpGet]
        public async Task<IActionResult> GetDichVu()
        {
            var dichVus = await _context.DichVu
                .Include(d => d.BangGiaDV)
                .ThenInclude(bg => bg.ChiTietBangGiaDV)
                .Select(d => new
                {
                    id = d.Id,
                    ten = d.Ten,
                    thoiGian = d.ThoiGian,
                    bangGiaDV = d.BangGiaDV.Select(bg => new
                    {
                        loai = bg.Loai,
                        chiTietBangGiaDV = bg.ChiTietBangGiaDV.Select(ct => new
                        {
                            chiPhi = ct.ChiPhi,
                            canNang = ct.CanNang
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            return Json(new { data = dichVus });
        }

        [HttpGet]
        [Route("admin/hoadondichvuad/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _hoaDonDichVuADService.Details(id);
            return View(details);
        }

        [HttpGet]
        [Route("admin/hoadondichvuad/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var hoaDon = await _hoaDonDichVuADService.Details(id);
            if (hoaDon == null)
            {
                TempData["Error"] = "Hóa đơn không tồn tại!";
                return RedirectToAction("Index");
            }

            var thuCungs = await _context.ThuCung.ToListAsync();
            ViewBag.ThuCungs = thuCungs;

            var dichVus = await _context.DichVu.ToListAsync();
            ViewBag.DichVus = dichVus;
            ViewBag.ThoiGianChamSocDB = hoaDon.ThoiGianChamSoc;
            ViewBag.NguoiTao = hoaDon.NguoiTao;
            ViewBag.BatDau = hoaDon.BatDau?.ToString("HH:mm") ?? "";
            ViewBag.KetThuc = hoaDon.KetThuc?.ToString("HH:mm") ?? "";

            ViewBag.SelectedDichVuIds = hoaDon.ChiTietHoaDonDichVu
                .Select(ct => ct.IdDichVu)
                .ToList();

            return View(hoaDon);
        }

        [HttpPost]
        [Route("admin/hoadondichvuad/edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind(Prefix = "ChiTietHoaDonDichVu")] List<ChiTietHoaDonDichVu> chiTietHoaDonDichVu, HoaDonDichVu model, string BatDau, string KetThuc, int ThoiGianChamSocDB)
        {
            if (id != model.Id)
            {
                TempData["Error"] = "ID không khớp!";
                return RedirectToAction("Index");
            }
            var hoaDon = await _hoaDonDichVuADService.Details(id);

            if (model.ThoiGianChamSoc == 0)
            {
                if (ThoiGianChamSocDB == 0)
                {
                    TempData["Error"] = "Yêu cầu chọn lại thời gian chăm sóc";
                    return await Edit(id);
                }
                model.ThoiGianChamSoc = ThoiGianChamSocDB;
            }

            if (chiTietHoaDonDichVu != null && chiTietHoaDonDichVu.Any())
            {
                var dichVuIds = chiTietHoaDonDichVu.Select(ct => ct.IdDichVu).Distinct();
                var dichVus = await _context.DichVu
                    .Where(d => dichVuIds.Contains(d.Id))
                    .ToListAsync();

                int totalTime = dichVus.Sum(d => d.ThoiGian) + model.ThoiGianChamSoc; // Giả sử ThoiGian là int? hoặc int

                // Kiểm tra điều kiện tổng thời gian
                if (totalTime > 660 && totalTime < 780 || totalTime > 1020)
                {
                    TempData["Error"] = "Yêu cầu chọn lại thời gian chăm sóc";
                    return await Edit(id);
                }
            }

            if (!ModelState.IsValid)
            {
                var thuCungs = await _context.ThuCung.ToListAsync();
                ViewBag.ThuCungs = thuCungs;

                var dichVus = await _context.DichVu.ToListAsync();
                ViewBag.DichVus = dichVus;

                ViewBag.NguoiTao = model.NguoiTao;
                ViewBag.BatDau = BatDau ?? "";
                ViewBag.KetThuc = KetThuc ?? "";

                ViewBag.SelectedDichVuIds = chiTietHoaDonDichVu
                    .Select(ct => ct.IdDichVu)
                    .ToList();
            }


            var result = await _hoaDonDichVuADService.Edit(id, model, chiTietHoaDonDichVu, BatDau, KetThuc);
            if (!result)
            {
                TempData["Error"] = "Cập nhật hóa đơn thất bại!";
                return View(model);
            }

            TempData["Success"] = "Cập nhật hóa đơn thành công!";
            return RedirectToAction("Details", "HoaDonDichVuAD", new { id = model.Id, area = "Admin" });
        }


        [Route("admin/datlich/listbydate/{ngayChamSoc}")]
        public async Task<IActionResult> ListByDate(DateTime ngayChamSoc)
        {
            var list = await _hoaDonDichVuADService.ListByDate(ngayChamSoc);
            return Json(new { Data = list });
        }


    }
}