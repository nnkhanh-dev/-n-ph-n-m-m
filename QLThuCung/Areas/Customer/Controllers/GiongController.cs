using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Customer.Services;
using QLThuCung.Models;
using System.Threading.Tasks;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class GiongController : Controller
    {
        private readonly IGiongKHService _giong;

        public GiongController(IGiongKHService giong)
        {
            _giong = giong;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("khachhang/giong/list")]
        public async Task<IActionResult> List()
        {
            var list = await _giong.List();
            return Json(new { Data = list });
        }
    }
}
