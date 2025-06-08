using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Customer.Services;
using System.Threading.Tasks;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class SanPhamController : Controller
    {
        private readonly ISanPhamKHService _sanPham;

        public SanPhamController(ISanPhamKHService sanPham)
        {
            _sanPham = sanPham;
        }

        [Route("khachhang/sanpham")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("/khachhang/sanpham/chitiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _sanPham.Details(id);
            return View(item);
        }
        [Route("/khachhang/sanpham/list")]
        public async Task<IActionResult> List() {
            var list = await _sanPham.List();
            return Json(new { Data = list });
        }

    }
}
