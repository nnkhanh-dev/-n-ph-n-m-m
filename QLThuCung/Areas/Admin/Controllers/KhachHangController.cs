using Microsoft.AspNetCore.Mvc;

namespace QLThuCung.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
