using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class HoaDonSPController : Controller
    {
        [Route("khachhang/hoadonsanpham")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
