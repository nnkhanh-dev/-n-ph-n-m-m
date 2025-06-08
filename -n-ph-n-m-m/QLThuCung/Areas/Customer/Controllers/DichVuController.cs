using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Customer.Services;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class DichVuController : Controller
    {
        private readonly IDichVuKHService _dichVu;

        public DichVuController(IDichVuKHService dichVu)
        {
            _dichVu = dichVu;
        }

        [Route("khachhang/dichvu")]
        public async Task<IActionResult> Index()
        {
            var list = await _dichVu.List();
            return View(list);
        }

        [Route("khachhang/dichvu/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _dichVu.Details(id);
            return PartialView("_Details",item);
        }

        [Route("khachhang/dichvu/list")]
        public async Task<IActionResult> List()
        {
            var list = await _dichVu.List();
            return Json(new { Data = list });
        }

    }
}
