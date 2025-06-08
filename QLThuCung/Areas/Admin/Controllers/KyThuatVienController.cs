using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Models;
using System.Security.AccessControl;

namespace QLThuCung.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KyThuatVienController : Controller
    {
        private readonly IKyThuatVienAdminService _kyThuatVien;

        public KyThuatVienController(IKyThuatVienAdminService kyThuatVien)
        {
            _kyThuatVien = kyThuatVien;
        }

        [Route("admin/kythuatvien")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Route("admin/kythuatvien/list")]
        public async Task<IActionResult> List()
        {
            var list = await _kyThuatVien.List();
            return Json(new { Data = list });
        }
        [Route("admin/kythuatvien/chitiet/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var details = await _kyThuatVien.Details(id);
            return View(details);
        }
        [Route("admin/kythuatvien/chinhsua/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var details = await _kyThuatVien.Details(id);
            return View(details);
        }
        [Route("admin/kythuatvien/chinhsua/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, NguoiDung model, IFormFile AnhMoi = null)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View();
            }
            if (AnhMoi != null && AnhMoi.Length > 0)
            {
                // Tạo đường dẫn đến thư mục Upload trong wwwroot
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload");

                // Kiểm tra thư mục Upload có tồn tại không, nếu không tạo mới
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Tạo tên file mới với tiền tố DateTime.Tick
                var fileName = $"{DateTime.Now.Ticks}_{AnhMoi.FileName}";
                var filePath = Path.Combine(uploadPath, fileName);

                // Lưu file vào thư mục
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await AnhMoi.CopyToAsync(fileStream);
                }

                // Cập nhật đường dẫn file vào model hoặc thực hiện các thao tác khác cần thiết
                model.AnhDaiDien = $"/Upload/{fileName}";
            }
            var result = await _kyThuatVien.Edit(id, model);
            if (result)
            {
                TempData["Success"] = "Chỉnh sửa thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Chỉnh sửa thất bại!";
                return View();
            }
        }
        [Route("admin/kythuatvien/xoa/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _kyThuatVien.Delete(id);
            return Json(new { success = result });
        }
        [Route("admin/kythuatvien/themmoi")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = new UserCTO();
            return View(user);
        }
        [Route("admin/kythuatvien/themmoi")]
        [HttpPost]
        public async Task<IActionResult> Create(UserCTO model, IFormFile AnhMoi = null)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View();
            }
            if (AnhMoi != null && AnhMoi.Length > 0)
            {
                // Tạo đường dẫn đến thư mục Upload trong wwwroot
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload");

                // Kiểm tra thư mục Upload có tồn tại không, nếu không tạo mới
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Tạo tên file mới với tiền tố DateTime.Tick
                var fileName = $"{DateTime.Now.Ticks}_{AnhMoi.FileName}";
                var filePath = Path.Combine(uploadPath, fileName);

                // Lưu file vào thư mục
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await AnhMoi.CopyToAsync(fileStream);
                }

                // Cập nhật đường dẫn file vào model hoặc thực hiện các thao tác khác cần thiết
                model.AnhDaiDien = $"/Upload/{fileName}";
            }
            var result = await _kyThuatVien.Create(model);
            if (result)
            {
                TempData["Success"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Thêm thất bại!";
                return View();
            }
        }
    }
}
