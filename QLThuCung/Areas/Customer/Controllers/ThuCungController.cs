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
    }
}
