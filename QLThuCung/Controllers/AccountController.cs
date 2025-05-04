using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Models;
using QLThuCung.ViewModels;
using System.Threading.Tasks;

namespace QLThuCung.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<NguoiDung> userManager,
                                 SignInManager<NguoiDung> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: /Account/Login
        [HttpGet]
        [Route("/dang-nhap")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [Route("/dang-nhap")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null)
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                if (roles.Contains("NhanVien"))
                    return RedirectToAction("Index", "Home", new { area = "Employee" });

                if (roles.Contains("KyThuatVien"))
                    return RedirectToAction("Index", "Home", new { area = "Teachnician" });

                if (roles.Contains("KhachHang"))
                    return RedirectToAction("Index", "Home", new { area = "Customer" });

                return RedirectToAction("Index", "Home"); // fallback
            }

            ModelState.AddModelError("", "Đăng nhập thất bại");
            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        [Route("/dang-ky")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [Route("/dang-ky")]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new NguoiDung
            {
                HoTen = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserName = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Gán role KhachHang
                await _userManager.AddToRoleAsync(user, "KhachHang");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home", new { area = "KhachHang" });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // Logout
        [HttpGet]
        [Route("/dang-xuat")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
