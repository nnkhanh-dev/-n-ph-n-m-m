using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class GiuongController : Controller
    {
        private readonly AppDbContext _context;

        public GiuongController(AppDbContext context)
        {
            _context = context;
        }
        [Route("khachhang/giuong/list")]
        public async Task<IActionResult> List()
        {
            var list = await _context.Giuong.ToListAsync();
            return Json(new {Data = list});
        }
    }
}
