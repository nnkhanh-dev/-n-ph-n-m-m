using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class GioHangController : Controller
    {
        [Route("/khachhang/giohang")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
