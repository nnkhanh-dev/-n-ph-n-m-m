using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Customer.Services;
using System.Threading.Tasks;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class LoaiController : Controller
    {
        private readonly ILoaiKHService _loai;

        public LoaiController(ILoaiKHService loai)
        {
            _loai = loai;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("khachhang/loai/list")]
        public async Task<IActionResult> List()
        {
            var list = await _loai.List();
            return Json(new { Data = list });
        }
    }
}
