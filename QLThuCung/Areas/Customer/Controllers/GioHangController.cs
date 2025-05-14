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
    public class GioHangController : Controller
    {
        private readonly IGioHangKHService _gioHang;
        private readonly UserManager<NguoiDung> _userManager;

        public GioHangController(IGioHangKHService gioHang, UserManager<NguoiDung> userManager)
        {
            _gioHang = gioHang;
            _userManager = userManager;
        }

        [Route("/khachhang/giohang")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("/khachhang/giohang/list")]
        public async Task<IActionResult> List()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var gioHang = await _gioHang.Find(user.Id);
            if (gioHang == null)
            {
                await _gioHang.Create(user.Id);
            }
            var list = await _gioHang.List(user.Id);
            return Json(new { Data = list });
        }

        [Route("/khachhang/giohang/taomoi")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChiTietGioHang model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }
            bool result;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng!" });
            }
            var gioHang = await _gioHang.Find(user.Id);
            if (gioHang == null)
            {
                result = await _gioHang.Create(user.Id);
                if (!result)
                {
                    return Json(new { success = false, message = "Không thể tạo giỏ hàng!" });
                }
                gioHang = await _gioHang.Find(user.Id);
                if(gioHang == null)
                {
                    return Json(new { success = false, message = "Không thể tìm thấy giỏ hàng!" });
                }
            }
          
            model.IdGioHang = gioHang.IdKhachHang;
            result = await _gioHang.CreateItem(model);
            if (!result)
            {
                return Json(new { success = false, message = "Thêm vào giỏ hàng thất bại!" });
            }
            return Json(new { success = true, message = "Thêm vào giỏ hàng thành công!" });
        }

        [Route("/khachhang/giohang/xoa/{idSP}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int idSP)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _gioHang.DeleteItem(idSP, user.Id);
            return Json(new { success = result });
        }
        [Route("/khachhang/giohang/chinhsua/")]
        public async Task<IActionResult> Edit([FromBody] ChiTietGioHang model)
        {
            var user = await _userManager.GetUserAsync(User);
            model.IdGioHang = user.Id;
            var result = await _gioHang.EditItem(model);
            return Json(new { success = result });
        }
    }
}
