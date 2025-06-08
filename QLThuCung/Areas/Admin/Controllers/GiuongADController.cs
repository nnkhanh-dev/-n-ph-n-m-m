using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;

namespace QLThuCung.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GiuongADController : Controller
    {
        private readonly AppDbContext _context;

        public GiuongADController(AppDbContext context)
        {
            _context = context;
        }
        [Route("admin/giuong/list")]
        public async Task<IActionResult> List()
        {
            var list = await _context.Giuong.ToListAsync();
            return Json(new { Data = list });
        }
    }
}
