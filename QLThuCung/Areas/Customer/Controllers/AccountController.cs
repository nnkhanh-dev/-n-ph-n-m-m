using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;
using System.Threading.Tasks;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class AccountController : Controller
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<NguoiDung> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Route("/khachhang/taikhoan")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
        [Route("/khachhang/taikhoan/chinhsua")]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
        [Route("/khachhang/taikhoan/chinhsua")]
        [HttpPost]
        public async Task<IActionResult> Edit(NguoiDung model, IFormFile AnhMoi = null)
        {
            if(!ModelState.IsValid)
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
            try
            {

                var user = await _userManager.GetUserAsync(User);

                // Cập nhật thông tin
                user.HoTen = model.HoTen;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.PhoneNumber;
                user.DiaChi = model.DiaChi;
                if (model.AnhDaiDien != null)
                {
                    user.AnhDaiDien = model.AnhDaiDien;
                }
                _context.NguoiDung.Update(user);
                var result = await _context.SaveChangesAsync() > 0;

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
            catch (Exception ex)
            {
                // Ghi log nếu cần: ex.Message
                TempData["Error"] = "Có lỗi khi chỉnh sửa!";
                return View();
            }
        }
    }
}
