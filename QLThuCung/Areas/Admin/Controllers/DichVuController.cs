using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class DichVuController : Controller
    {
        private readonly IDichVuAdminService _dichVu;
        private readonly AppDbContext _context;

        public DichVuController(IDichVuAdminService dichVu, AppDbContext context)
        {
            _dichVu = dichVu;
            _context = context;
        }

        [Route("/admin/dichvu")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("admin/dichvu/list")]
        public async Task<IActionResult> List()
        {
            var list = await _dichVu.List();
            return Json(new { Data = list });
        }

        [Route("admin/dichvu/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new DichVu());
        }


        [Route("admin/dichvu/create")]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Ten,ThoiGian,MoTa,TrangThai")] DichVu model, List<IFormFile>? NewImages, string? LoaiTinhGia, decimal? GiaCoDinh, decimal[]? CustomCanNangList, decimal[]? CustomChiPhiList)
        {
            model.NgayTao = DateTime.Now;
            model.AnhDichVu = new List<AnhDichVu>();
            model.BangGiaDV = new List<BangGiaDV>();

            if (NewImages != null && NewImages.Count > 0)
            {
                foreach (var file in NewImages)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var path = Path.Combine("wwwroot/Upload", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        model.AnhDichVu.Add(new AnhDichVu
                        {
                            DuongDan = "/Upload/" + fileName
                        });
                    }
                }
            }

            var success = await _dichVu.Create(model);
            if (!success)
            {
                TempData["Error"] = "Tạo mới thất bại!";
                return View(model);
            }

            // Xử lý bảng giá
            if (!string.IsNullOrEmpty(LoaiTinhGia))
            {
                int loaiTinhGia = int.Parse(LoaiTinhGia);
                var bangGiaDV = new BangGiaDV
                {
                    IdDichVu = model.Id,
                    NgayTao = DateTime.Now,
                    Loai = loaiTinhGia
                };
                _context.BangGiaDV.Add(bangGiaDV);
                await _context.SaveChangesAsync();

                if (loaiTinhGia == 0) // Cố định
                {
                    if (GiaCoDinh.HasValue)
                    {
                        var chiTiet = new ChiTietBangGiaDV
                        {
                            CanNang = 0,
                            ChiPhi = GiaCoDinh.Value,
                            IdBangGiaDV = bangGiaDV.Id
                        };
                        _context.ChiTietBangGiaDV.Add(chiTiet);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        TempData["Error"] = "Vui lòng nhập giá cố định!";
                        return View(model);
                    }
                }
                else if (loaiTinhGia == 1) // Theo cân nặng
                {
                    if (CustomCanNangList == null || CustomChiPhiList == null || CustomCanNangList.Length != CustomChiPhiList.Length || CustomCanNangList.Length == 0)
                    {
                        TempData["Error"] = "Vui lòng nhập ít nhất một cặp cân nặng và giá!";
                        return View(model);
                    }

                    var chiTietListFromCustom = new List<ChiTietBangGiaDV>();
                    for (int i = 0; i < CustomCanNangList.Length; i++)
                    {
                        chiTietListFromCustom.Add(new ChiTietBangGiaDV
                        {
                            CanNang = CustomCanNangList[i],
                            ChiPhi = CustomChiPhiList[i],
                            IdBangGiaDV = bangGiaDV.Id
                        });
                    }
                    _context.ChiTietBangGiaDV.AddRange(chiTietListFromCustom);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                TempData["Error"] = "Vui lòng chọn loại hình tính giá!";
                return View(model);
            }

            TempData["Success"] = "Tạo mới thành công!";
            return RedirectToAction("Index");
        }


        [Route("admin/dichvu/chitiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _dichVu.Details(id);
            return View(details);
        }

        [Route("admin/dichvu/chinhsua/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _context.DichVu
                                .Include(d => d.AnhDichVu)
                                .Include(d => d.BangGiaDV)
                                    .ThenInclude(b => b.ChiTietBangGiaDV)
                                .FirstOrDefaultAsync(d => d.Id == id);

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [Route("admin/dichvu/chinhsua/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,ThoiGian,MoTa,TrangThai")] DichVu model, List<IFormFile>? NewImages, decimal? GiaCoDinh, decimal[]? CustomCanNangList, decimal[]? CustomChiPhiList)
        {
            var dichVu = await _context.DichVu
                .Include(d => d.AnhDichVu)
                .Include(d => d.BangGiaDV)
                    .ThenInclude(b => b.ChiTietBangGiaDV)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dichVu == null)
            {
                return NotFound();
            }
            int currentTime = dichVu.ThoiGian;
            dichVu.Ten = model.Ten;
            dichVu.ThoiGian = model.ThoiGian;
            dichVu.MoTa = model.MoTa;
            dichVu.TrangThai = model.TrangThai;

            if (NewImages != null && NewImages.Count > 0)
            {
                foreach (var file in NewImages)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var path = Path.Combine("wwwroot/Upload", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        dichVu.AnhDichVu.Add(new AnhDichVu
                        {
                            DuongDan = "/Upload/" + fileName,
                            IdDichVu = dichVu.Id
                        });
                    }
                }
            }

            var bangGiaDV = dichVu.BangGiaDV.FirstOrDefault();
            if (bangGiaDV != null)
            {
                if (bangGiaDV.Loai == 0)
                {
                    if (GiaCoDinh.HasValue)
                    {
                        var chiTiet = bangGiaDV.ChiTietBangGiaDV.FirstOrDefault();
                        if (chiTiet != null)
                        {
                            chiTiet.ChiPhi = GiaCoDinh.Value;
                        }
                        else
                        {
                            bangGiaDV.ChiTietBangGiaDV.Add(new ChiTietBangGiaDV
                            {
                                CanNang = 0,
                                ChiPhi = GiaCoDinh.Value,
                                IdBangGiaDV = bangGiaDV.Id
                            });
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Vui lòng nhập giá cố định!";
                        return View(dichVu);
                    }
                }
                else if (bangGiaDV.Loai == 1)
                {
                    if (CustomCanNangList == null || CustomChiPhiList == null || CustomCanNangList.Length != CustomChiPhiList.Length || CustomCanNangList.Length == 0)
                    {
                        TempData["Error"] = "Vui lòng nhập ít nhất một cặp cân nặng và giá!";
                        return View(dichVu);
                    }

                    _context.ChiTietBangGiaDV.RemoveRange(bangGiaDV.ChiTietBangGiaDV);

                    var chiTietList = new List<ChiTietBangGiaDV>();
                    for (int i = 0; i < CustomCanNangList.Length; i++)
                    {
                        chiTietList.Add(new ChiTietBangGiaDV
                        {
                            CanNang = CustomCanNangList[i],
                            ChiPhi = CustomChiPhiList[i],
                            IdBangGiaDV = bangGiaDV.Id
                        });
                    }
                    _context.ChiTietBangGiaDV.AddRange(chiTietList);
                }
            }
            else
            {
                TempData["Error"] = "Không có bảng giá để cập nhật!";
                return View(dichVu);
            }

            try
            {
                bool success = await _dichVu.Edit(id, dichVu, currentTime); // Gọi service
                if (success)
                {
                    TempData["Success"] = "Cập nhật dịch vụ thành công!";
                }
                else
                {
                    TempData["Error"] = "Cập nhật thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Cập nhật thất bại: " + ex.Message;
                return View(dichVu);
            }

            return RedirectToAction("Index");
        }


        // -------------------- Xóa Ảnh: chỉnh cần nhấn vào button X là xóa luôn trong csdl------------------------
        [Route("admin/dichvu/deleteimage/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var anhDichVu = await _context.AnhDichVu.FirstOrDefaultAsync(a => a.Id == id);

            if (anhDichVu == null)
            {
                return Json(new { success = false, message = "Hình ảnh không tồn tại!" });
            }

            // Xóa file trên server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", anhDichVu.DuongDan.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.AnhDichVu.Remove(anhDichVu);
            bool success = await _context.SaveChangesAsync() > 0;

            return Json(new { success = success, message = success ? "Xóa hình ảnh thành công!" : "Xóa hình ảnh thất bại!" });
        }


        [Route("admin/dichvu/xoa/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dichVu.Delete(id);
            return Json(new { success = result });
        }
    }
}