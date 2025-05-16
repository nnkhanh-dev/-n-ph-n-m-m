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
    public class ThuCungController : Controller
    {
        private readonly IThuCungKHService _thuCung;
        private readonly UserManager<NguoiDung> _userManager;

        public ThuCungController(IThuCungKHService thuCung, UserManager<NguoiDung> userManager)
        {
            _thuCung = thuCung;
            _userManager = userManager;
        }
        [Route("khachhang/thucung")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.IdNguoiDung = user.Id;
            return View();
        }
        [Route("khachhang/thucung/chitiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _thuCung.Details(id);
            return View(details);
        }
        [Route("khachhang/thucung/list/{id}")]
        public async Task<IActionResult> List(string id)
        {
            var list = await _thuCung.List(id);
            return Json( new {Data = list });
        }
        [Route("khachhang/thucung/xoa/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _thuCung.Delete(id);
            return Json(new { success = result });
        }
        [Route("khachhang/thucung/chinhsua/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var details = await _thuCung.Details(id);
            return View(details);
        }
        [Route("khachhang/thucung/chinhsua/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ThuCung model, IFormFile AnhMoi = null)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View(model);
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
          
            var result = await _thuCung.Edit(id, model);
            if (!result)
            {
                TempData["Error"] = "Chỉnh sửa thất bại!";
                return View(model);
            }
            TempData["Success"] = "Chỉnh sửa thành công!";
            return RedirectToAction("Index");
        }

        [Route("khachhang/thucung/taomoi/")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var thuCung = new ThuCung();
            return View(thuCung);
        }

        [Route("khachhang/thucung/taomoi/")]
        [HttpPost]
        public async Task<IActionResult> Create(ThuCung model, IFormFile AnhMoi = null)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View(model);
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
            var user = await _userManager.GetUserAsync(User);
            model.NguoiTao = user.Id;
            model.IdKhachHang = user.Id;
            var result = await _thuCung.Create(model);
            if (!result)
            {
                TempData["Error"] = "Tạo mới thất bại!";
                return View(model);
            }
            TempData["Success"] = "Tạo mới thành công!";
            return RedirectToAction("Index");
        }
    }
}
