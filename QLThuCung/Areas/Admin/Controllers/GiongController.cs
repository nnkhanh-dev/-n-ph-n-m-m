using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.Models;
using System.Security.AccessControl;

namespace QLThuCung.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GiongController : Controller
    {
        private readonly IGiongAdminService _giong;

        public GiongController(IGiongAdminService giong)
        {
            _giong = giong;
        }

        [Route("admin/giong")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Route("admin/giong/list")]
        public async Task<IActionResult> List()
        {
            var list = await _giong.List();
            return Json(new { Data = list });
        }
        [Route("admin/giong/chitiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _giong.Details(id);
            return View(details);
        }
        [Route("admin/giong/chinhsua/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var details = await _giong.Details(id);
            return View(details);
        }
        [Route("admin/giong/chinhsua/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Giong model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View(model);
            }

            var result = await _giong.Edit(id, model);
            if (result)
            {
                TempData["Success"] = "Chỉnh sửa thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Chỉnh sửa thất bại!";
                return View(model);
            }
        }
        [Route("admin/giong/xoa/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _giong.Delete(id);
            return Json(new { success = result });
        }
        [Route("admin/giong/themmoi")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = new Giong();
            return View(user);
        }
        [Route("admin/giong/themmoi")]
        [HttpPost]
        public async Task<IActionResult> Create(Giong model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View(model);
            }
            var result = await _giong.Create(model);
            if (result)
            {
                TempData["Success"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Thêm thất bại!";
                return View(model);
            }
        }
    }
}

