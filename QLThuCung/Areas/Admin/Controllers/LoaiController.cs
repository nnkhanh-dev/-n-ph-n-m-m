using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.Models;
using System.Security.AccessControl;

namespace QLThuCung.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LoaiController : Controller
    {
        private readonly ILoaiAdminService _loai;

        public LoaiController(ILoaiAdminService loai)
        {
            _loai = loai;
        }

        [Route("admin/loai")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Route("admin/loai/list")]
        public async Task<IActionResult> List()
        {
            var list = await _loai.List();
            return Json(new { Data = list });
        }
        [Route("admin/loai/chitiet/{id}")]
        public async Task<IActionResult> Details(int  id)
        {
            var details = await _loai.Details(id);
            return View(details);
        }
        [Route("admin/loai/chinhsua/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var details = await _loai.Details(id);
            return View(details);
        }
        [Route("admin/loai/chinhsua/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Loai model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View(model);
            }
            //if (AnhMoi != null && AnhMoi.Length > 0)
            //{
            //    // Tạo đường dẫn đến thư mục Upload trong wwwroot
            //    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload");

            //    // Kiểm tra thư mục Upload có tồn tại không, nếu không tạo mới
            //    if (!Directory.Exists(uploadPath))
            //    {
            //        Directory.CreateDirectory(uploadPath);
            //    }

            //    // Tạo tên file mới với tiền tố DateTime.Tick
            //    var fileName = $"{DateTime.Now.Ticks}_{AnhMoi.FileName}";
            //    var filePath = Path.Combine(uploadPath, fileName);

            //    // Lưu file vào thư mục
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await AnhMoi.CopyToAsync(fileStream);
            //    }

            //    ////Cập nhật đường dẫn file vào model hoặc thực hiện các thao tác khác cần thiết
            //    //model.AnhMinhHoa = $"/Upload/{fileName}";
            //}
            var result = await _loai.Edit(id, model);
            if (result)
            {
                TempData["Success"] = "Chỉnh sửa thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Chỉnh sửa thất bại!";
                return View(model);
            }
        }
        [Route("admin/loai/xoa/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loai.Delete(id);
            return Json(new { success = result });
        }
        [Route("admin/loai/themmoi")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = new Loai();
            return View(user);
        }
        [Route("admin/loai/themmoi")]
        [HttpPost]
        public async Task<IActionResult> Create(Loai model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View();
            }
            //if (AnhMoi != null && AnhMoi.Length > 0)
            //{
            //    // Tạo đường dẫn đến thư mục Upload trong wwwroot
            //    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload");

            //    // Kiểm tra thư mục Upload có tồn tại không, nếu không tạo mới
            //    if (!Directory.Exists(uploadPath))
            //    {
            //        Directory.CreateDirectory(uploadPath);
            //    }

            //    // Tạo tên file mới với tiền tố DateTime.Tick
            //    var fileName = $"{DateTime.Now.Ticks}_{AnhMoi.FileName}";
            //    var filePath = Path.Combine(uploadPath, fileName);

            //    // Lưu file vào thư mục
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await AnhMoi.CopyToAsync(fileStream);
            //    }

            //    // Cập nhật đường dẫn file vào model hoặc thực hiện các thao tác khác cần thiết
            //    model.AnhMinhHoa = $"/Upload/{fileName}";
            //}
            var result = await _loai.Create(model);
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

