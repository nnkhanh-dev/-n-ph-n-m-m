using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Customer.Services;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class ThuCungController : Controller
    {
        private readonly IThuCungKHService _thuCung;

        public ThuCungController(IThuCungKHService thuCung)
        {
            _thuCung = thuCung;
        }
        [Route("khachhang/thucung")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("khachhang/thucung/chitiet")]
        public IActionResult Details()
        {
            return View();
        }
        [Route("khachhang/thucung/list/{id}")]
        public async Task<IActionResult> List(string id)
        {
            var list = await _thuCung.List(id);
            return Json( new {Data = list });
        }
    }
}
