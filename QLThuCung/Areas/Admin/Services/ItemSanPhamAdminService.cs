using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public class ItemSanPhamAdminService : ISanphamService
    {
        private readonly AppDbContext _context;
        public ItemSanPhamAdminService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SanPhamResponse>> List()
        {
            return await _context.SanPham
                .Select(sp => new SanPhamResponse
                {
                    Id = sp.Id,
                    Ten = sp.Ten,
                    MoTa = sp.MoTa,
                    SoLuong = sp.SoLuong,
                    Gia = sp.Gia,
                    GiamGia = sp.GiamGia,
                    NgayTao = sp.NgayTao,
                    IdDanhMuc = sp.IdDanhMuc,
                    TenDanhMuc = sp.DanhMuc.Ten,
                    AnhSanPham = sp.AnhSanPham
                }).ToListAsync();
        }
        public async Task<SanPhamResponse> Details(string id)
        {
            if (!int.TryParse(id, out int maSp))
                return null;

            return await _context.SanPham
                .Where(sp => sp.Id == maSp)
                .Select(sp => new SanPhamResponse
                {
                    Id = sp.Id,
                    Ten = sp.Ten,
                    MoTa = sp.MoTa,
                    SoLuong = sp.SoLuong,
                    Gia = sp.Gia,
                    GiamGia = sp.GiamGia,
                    NgayTao = sp.NgayTao,
                    IdDanhMuc = sp.IdDanhMuc,
                    TenDanhMuc = sp.DanhMuc.Ten,
                    AnhSanPham = sp.AnhSanPham
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> Create(SanPhamResquest model)
        {
            try
            {
                var sp = new SanPham
                {
                    Ten = model.Ten,
                    MoTa = model.MoTa,
                    IdDanhMuc = model.IdDanhMuc,
                    Gia = model.Gia,
                    SoLuong = model.SoLuong,
                    GiamGia = model.GiamGia,
                    NgayTao = DateTime.Now
                };

                await _context.SanPham.AddAsync(sp);
                await _context.SaveChangesAsync(); // Lúc này sp.Id mới được tạo

                if (model.ListAnh != null && model.ListAnh.Any())
                {
                    foreach (string image in model.ListAnh)
                    {
                        var img = new AnhSanPham
                        {
                            DuongDan = image,
                            IdSanPham = sp.Id
                        };
                        await _context.AnhSanPham.AddAsync(img);
                    }

                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần
                return false;
            }
        }

        public async Task<bool> Edit(string id, SanPhamResquest model, List<string> anhmoi)
        {
            var sanPham = await _context.SanPham
                .Include(s => s.AnhSanPham)
                .FirstOrDefaultAsync(s => s.Id == Int32.Parse(id));

            if (sanPham == null) return false;

            // 1. Cập nhật thông tin cơ bản
            sanPham.Ten = model.Ten;
            sanPham.MoTa = model.MoTa;
            sanPham.SoLuong = model.SoLuong;
            sanPham.Gia = model.Gia;
            sanPham.GiamGia = model.GiamGia;
            sanPham.IdDanhMuc = model.IdDanhMuc;
            

            // 2. Xóa ảnh bị xóa
            if (model.AnhSanPham.Count < sanPham.AnhSanPham.Count)
            {
                List<string> listImage  = sanPham.AnhSanPham.Where(x=>!model.AnhSanPham.Contains(x)).Select(x=>x.DuongDan).ToList();
                foreach (var duongDan in listImage)
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", duongDan.TrimStart('/'));

                    // Xóa file vật lý nếu tồn tại
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    // Xóa trong DB
                    var anh = sanPham.AnhSanPham.FirstOrDefault(a => a.DuongDan != duongDan);
                    if (anh != null)
                    {
                        _context.AnhSanPham.Remove(anh); // EF sẽ tự loại khỏi navigation
                    }
                }
            }
            sanPham.AnhSanPham = model.AnhSanPham;

            // 3. Thêm ảnh mới (đã upload từ Controller và nằm trong model.AnhSanPham)
            if (model.AnhSanPham != null && model.AnhSanPham.Any())
            {
                foreach (var duongDanMoi in anhmoi)
                {
                    sanPham.AnhSanPham.Add(new AnhSanPham
                    {
                        IdSanPham = sanPham.Id,
                        DuongDan = duongDanMoi
                    });
                }
            }

            // 4. Lưu thay đổi
            _context.SanPham.Update(sanPham);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> Delete(string id)
        {
            if (!int.TryParse(id, out int productId)) return false;

            try
            {
                var sp = await _context.SanPham
                    .Include(p => p.AnhSanPham) // load luôn ảnh
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (sp == null) return false;

                // Nếu muốn xóa ảnh trong ổ đĩa (tùy chọn)
                foreach (var img in sp.AnhSanPham)
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.DuongDan.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                // Xóa ảnh trong DB
                _context.AnhSanPham.RemoveRange(sp.AnhSanPham);

                // Xóa sản phẩm
                _context.SanPham.Remove(sp);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Xóa sản phẩm] Lỗi: {ex.Message}");
                return false;
            }
        }

    }
}
