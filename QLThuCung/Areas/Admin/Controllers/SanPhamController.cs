using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Areas.Customer.Services;
using QLThuCung.Models;
using System.Security.AccessControl;

namespace QLThuCung.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SanPhamController : Controller
    {
        private readonly ISanphamService _sanpham;
        private readonly IDanhMucAdminService _danhMuc;
        private readonly IAnhSanPhamAdminService _anhsanpham;


        public SanPhamController(ISanphamService sanpham, IDanhMucAdminService danhmuc, IAnhSanPhamAdminService anhsanpham)
        {
            _sanpham = sanpham;
            _danhMuc = danhmuc;
            _anhsanpham = anhsanpham;
        }

        [Route("admin/sanpham")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("admin/sanpham/list")]
        public async Task<IActionResult> List()
        {
            var list = await _sanpham.List();
            return Json(new { Data = list });
        }

        [Route("admin/sanpham/chitiet/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var details = await _sanpham.Details(id);
            ViewBag.Id = id;
            return View(details);
        }

        [Route("admin/sanpham/chinhsua/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var detail = await _sanpham.Details(id);
            ViewBag.Details = detail;
            var req = new SanPhamResquest();
            req.MoTa = detail.MoTa;
            ViewBag.Id = id;
            req.IdDanhMuc = detail.IdDanhMuc;
            req.AnhSanPham = detail.AnhSanPham;

            //var danhMucs = await _danhMuc.List();
            //ViewBag.DanhMuc = danhMucs.Select(dm => new SelectListItem
            //{
            //    Value = dm.Id.ToString(),
            //    Text = dm.Ten
            //}).ToList();

            var danhMucs = await _danhMuc.List();

            // Tạo danh sách SelectListItem, set selected theo IdDanhMuc
            ViewBag.DanhMuc = danhMucs.Select(dm => new SelectListItem
            {
                Value = dm.Id.ToString(),
                Text = dm.Ten,
                Selected = (dm.Id == req.IdDanhMuc)
            }).ToList();

            return View(req);
        }



        [Route("admin/sanpham/themmoi")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = new SanPhamResquest();
            var danhMucs = await _danhMuc.List();
            ViewBag.DanhMuc = danhMucs.Select(dm => new SelectListItem
            {
                Value = dm.Id.ToString(),
                Text = dm.Ten
            }).ToList();
            return View(user);
        }

        [HttpPost("admin/sanpham/chinhsua/{id}")]
        public async Task<IActionResult> Edit(string id, SanPhamResquest model, IFormFile[] AnhMoi=null)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                // Load lại danh mục
                var detail = await _sanpham.Details(id);
                ViewBag.Details = detail;
                var req = new SanPhamResquest();
                req.MoTa = detail.MoTa;

                req.IdDanhMuc = detail.IdDanhMuc;
                var danhMucs = await _danhMuc.List();

                // Tạo danh sách SelectListItem, set selected theo IdDanhMuc
                ViewBag.DanhMuc = danhMucs.Select(dm => new SelectListItem
                {
                    Value = dm.Id.ToString(),
                    Text = dm.Ten,
                    Selected = (dm.Id == req.IdDanhMuc)
                }).ToList();

                return View(req);
            }
            

            var danhSachAnh = new List<string>();

            if (AnhMoi != null && AnhMoi.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (var file in AnhMoi)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        danhSachAnh.Add($"/Upload/{fileName}");
                    }
                }
            }
            //model.AnhSanPham = danhSachAnh;

            //var result = await _sanpham.Edit(id, model, danhSachXoa);

            var result = await _sanpham.Edit(id, model, danhSachAnh);

            if (result)
            {
                TempData["Success"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Cập nhật thất bại!";
            return View(model);
        }

        [Route("admin/sanpham/themmoi")]
        [HttpPost]
        public async Task<IActionResult> Create(SanPhamResquest model, IFormFile[] AnhMoi = null)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";

                var danhMucs = await _danhMuc.List();
                ViewBag.DanhMuc = danhMucs.Select(dm => new SelectListItem
                {
                    Value = dm.Id.ToString(),
                    Text = dm.Ten
                }).ToList();
                return View(model);
            }

            var danhSachAnh = new List<string>();

            if (AnhMoi != null && AnhMoi.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (var file in AnhMoi)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        danhSachAnh.Add($"/Upload/{fileName}");
                    }
                }
            }
            model.ListAnh = danhSachAnh;

            // Gửi dữ liệu + danh sách ảnh cho service
            var result = await _sanpham.Create(model);

            if (result)
            {
                TempData["Success"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Thêm thất bại!";
                var danhMucs = await _danhMuc.List();
                ViewBag.DanhMuc = danhMucs.Select(dm => new SelectListItem
                {
                    Value = dm.Id.ToString(),
                    Text = dm.Ten
                }).ToList();
                return View(model);
            }
        }

        [Route("admin/sanpham/xoa/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _sanpham.Delete(id);
            return Json(new { success = result });
        }
    }
}
