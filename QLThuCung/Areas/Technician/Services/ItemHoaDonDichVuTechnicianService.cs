using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Technician.Services;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Technician.Services
{
    public class ItemHoaDonDichVuTechnicianService : IHoaDonDichVuTechnicianService
    {
        private readonly AppDbContext _context;

        public ItemHoaDonDichVuTechnicianService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HoaDonDichVu>> List()
        {
            return await _context.HoaDonDichVu
                .Include(h => h.ThuCung)
                    .ThenInclude(t => t.KhachHang) // Sử dụng navigation property
                .Include(h => h.ChiTietHoaDonDichVu)
                    .ThenInclude(c => c.DichVu)
                .ToListAsync();
        }

        public async Task<HoaDonDichVu> Details(int id)
        {
            var hoaDon = await _context.HoaDonDichVu
                                        .Include(h => h.ThuCung)
                                            .ThenInclude(t => t.KhachHang)
                                        .Include(h => h.ChiTietHoaDonDichVu)
                                            .ThenInclude(ct => ct.DichVu)
                                        .FirstOrDefaultAsync(h => h.Id == id);

            return hoaDon;
        }

        public async Task<bool> Create(HoaDonDichVu model)
        {
            if (model == null)
            {
                return false;
            }

            // Bắt đầu Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hoaDon = new HoaDonDichVu
                {
                    IdThuCung = model.IdThuCung,
                    TrangThai = model.PhuongThucThanhToan == 1 ? -100 : 0,
                    PhuongThucThanhToan = model.PhuongThucThanhToan,
                    NgayChamSoc = model.NgayChamSoc,
                    ThoiGianChamSoc = model.ThoiGianChamSoc,
                    NgayTao = DateTime.Now,
                    NguoiTao = model.NguoiTao,
                    MaThanhToan = model.MaThanhToan
                };

                _context.HoaDonDichVu.Add(hoaDon);
                var hoaDonResult = await _context.SaveChangesAsync() > 0;
                if (!hoaDonResult)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                foreach (var item in model.ChiTietHoaDonDichVu)
                {
                    var chiTiet = new ChiTietHoaDonDichVu
                    {
                        IdHoaDon = hoaDon.Id,
                        IdDichVu = item.IdDichVu,
                        DonGia = item.DonGia
                    };
                    _context.ChiTietHoaDonDichVu.Add(chiTiet);
                }

                var chiTietResult = await _context.SaveChangesAsync() > 0;
                if (!chiTietResult)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Commit transaction nếu tất cả lưu thành công
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                await transaction.RollbackAsync();
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var hoaDon = await _context.HoaDonDichVu
                .Include(h => h.ChiTietHoaDonDichVu)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hoaDon == null)
            {
                throw new KeyNotFoundException($"Hóa đơn với ID {id} không tồn tại.");
            }

            _context.ChiTietHoaDonDichVu.RemoveRange(hoaDon.ChiTietHoaDonDichVu);
            _context.HoaDonDichVu.Remove(hoaDon);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<NguoiDung>> GetKhachHangs(string term = null)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "KhachHang");
            if (role == null) return new List<NguoiDung>();

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var query = _context.NguoiDung
                .Where(u => userIds.Contains(u.Id));

            if (!string.IsNullOrEmpty(term))
            {
                var normalizedTerm = term.Normalize(System.Text.NormalizationForm.FormD)
                    .ToLower()
                    .Replace("[đ]", "d");

                query = query.Where(u => u.HoTen.Normalize(System.Text.NormalizationForm.FormD)
                    .ToLower()
                    .Replace("[đ]", "d")
                    .Contains(normalizedTerm) ||
                    u.Id.Normalize(System.Text.NormalizationForm.FormD)
                    .ToLower()
                    .Replace("[đ]", "d")
                    .Contains(normalizedTerm));
            }

            var khachHangs = await query
                .Take(50)
                .ToListAsync();

            return khachHangs;
        }

        public async Task<decimal> TotalPrice(HoaDonDichVu model)
        {
            decimal totalPrice = 0;
            if (model == null)
            {
                return totalPrice;
            }
            foreach (var item in model.ChiTietHoaDonDichVu)
            {
                totalPrice += item.DonGia;
            }
            return totalPrice;
        }

        public async Task<bool> Edit(int id, HoaDonDichVu hoaDon, List<ChiTietHoaDonDichVu> chiTietHoaDonDichVu, string batDau, string ketThuc)
        {
            var existingHoaDon = await _context.HoaDonDichVu
                .Include(h => h.ChiTietHoaDonDichVu)
                .Include(h => h.ThuCung)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (existingHoaDon == null)
            {
                return false;
            }

            // Cập nhật các thuộc tính của hóa đơn
            existingHoaDon.IdThuCung = hoaDon.IdThuCung;
            existingHoaDon.NgayChamSoc = hoaDon.NgayChamSoc;
            existingHoaDon.ThoiGianChamSoc = hoaDon.ThoiGianChamSoc;
            existingHoaDon.PhuongThucThanhToan = hoaDon.PhuongThucThanhToan;
            existingHoaDon.TrangThai = hoaDon.TrangThai;
            existingHoaDon.IdGiuong = hoaDon.IdGiuong;

            // Xử lý thời gian bắt đầu và kết thúc
            if (hoaDon.TrangThai == 1 && !string.IsNullOrEmpty(batDau))
            {
                if (TimeSpan.TryParse(batDau, out TimeSpan batDauTime) && existingHoaDon.BatDau == null)
                {
                    DateTime batDauDateTime = hoaDon.NgayChamSoc.Add(batDauTime);
                    existingHoaDon.BatDau = batDauDateTime;
                }
            }
            else if (hoaDon.TrangThai == 2 && !string.IsNullOrEmpty(ketThuc))
            {
                if (TimeSpan.TryParse(ketThuc, out TimeSpan ketThucTime) && existingHoaDon.KetThuc == null)
                {
                    DateTime ketThucDateTime = hoaDon.NgayChamSoc.Add(ketThucTime);
                    existingHoaDon.KetThuc = ketThucDateTime;
                }
            }

            // Xóa các ChiTietHoaDonDichVu cũ
            if (existingHoaDon.ChiTietHoaDonDichVu != null && existingHoaDon.ChiTietHoaDonDichVu.Any())
            {
                _context.ChiTietHoaDonDichVu.RemoveRange(existingHoaDon.ChiTietHoaDonDichVu);
            }

            // Thêm mới các ChiTietHoaDonDichVu từ form
            if (chiTietHoaDonDichVu != null && chiTietHoaDonDichVu.Any())
            {
                foreach (var item in chiTietHoaDonDichVu)
                {
                    var chiTiet = new ChiTietHoaDonDichVu
                    {
                        IdHoaDon = id,
                        IdDichVu = item.IdDichVu,
                        DonGia = item.DonGia
                    };
                    _context.ChiTietHoaDonDichVu.Add(chiTiet);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<HoaDonDichVu>> ListByCustomer(string id)
        {
            var list = await _context.HoaDonDichVu.Include(x => x.ChiTietHoaDonDichVu)
                                                  .Include(x => x.DanhGia)
                                                  .Include(x => x.ThuCung)
                                                    .ThenInclude(x => x.KhachHang)
                                                  .Where(x => x.ThuCung.KhachHang.Id == id && x.TrangThai != -100)
                                                  .ToListAsync();
            return list;
        }

        public async Task<IEnumerable<HoaDonDichVu>> ListByDate(DateTime NgayChamSoc)
        {
            var list = await _context.HoaDonDichVu.Include(x => x.ChiTietHoaDonDichVu)
                                                  .ThenInclude(x => x.DichVu)
                                                  .Where(x => x.NgayChamSoc == NgayChamSoc && x.TrangThai != -1 && x.TrangThai != -100)
                                                  .ToListAsync();
            return list;
        }
    }
}