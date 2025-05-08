using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class AccountController : Controller
    {
        [Route("/khachhang/taikhoan")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
