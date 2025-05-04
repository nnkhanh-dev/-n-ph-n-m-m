using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class SanPhamController : Controller
    {
        [Route("khachhang/sanpham")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
