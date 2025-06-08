using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Customer.Services;

namespace QLThuCung.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class DanhMucController : Controller
    {
        private readonly IDanhMucService _danhMuc;

        public DanhMucController(IDanhMucService danhMuc)
        {
            _danhMuc = danhMuc;
        }
        [Route("khachhang/danhmuc/list")]
        public async Task<IActionResult> List()
        {
            var list = await _danhMuc.List();
            return Json(new { Data = list });
        }
    }
}
