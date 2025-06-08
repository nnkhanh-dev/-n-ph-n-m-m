using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public class ItemDichVuAdminService : IDichVuAdminService
    {
        private readonly AppDbContext _context;

        public ItemDichVuAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(DichVu model)
        {
            if (model == null)
            {
                return false;
            }
            try
            {
                _context.DichVu.Add(model);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.DichVu.FindAsync(id);
            if (entity == null)
                return false;

            try
            {
                _context.DichVu.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<DichVu> Details(int id)
        {
            return await _context.DichVu
                .Include(d => d.AnhDichVu)
                .Include(d => d.BangGiaDV)
                    .ThenInclude(x => x.ChiTietBangGiaDV)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> Edit(int id, DichVu model, int currentTime)
        {
            if (model == null || model.Id != id)
                return false;

            var existing = await _context.DichVu.FindAsync(id);
            if (existing == null)
                return false;
            
            try
            {
                // Cập nhật các trường cần thiết
                existing.Ten = model.Ten;
                existing.ThoiGian = model.ThoiGian;
                existing.MoTa = model.MoTa;
                existing.TrangThai = model.TrangThai;
                //existing.NgayTao = model.NgayTao;
                existing.AnhDichVu = model.AnhDichVu;
                //existing.BangGiaDV = model.BangGiaDV;

                _context.DichVu.Update(existing);
                int changes = await _context.SaveChangesAsync();
                if (changes > 0) // Chỉ thực hiện logic nếu update thành công
                {
                    if (existing.ThoiGian != currentTime)
                    {
                        // Lấy tất cả HoaDonDichVu liên quan qua ChiTietHoaDonDichVu
                        var hoaDonDichVuIds = await _context.ChiTietHoaDonDichVu
                            .Where(ct => ct.IdDichVu == model.Id)
                            .Select(ct => ct.IdHoaDon)
                            .Distinct()
                            .ToListAsync();

                        var hoaDons = await _context.HoaDonDichVu
                            .Where(hd => hoaDonDichVuIds.Contains(hd.Id))
                            .ToListAsync();

                        foreach (var hoaDon in hoaDons)
                        {
                            if (hoaDon.TrangThai == 0) // "Chờ xử lý"
                            {
                                int thoiGianChamSoc = hoaDon.ThoiGianChamSoc;
                                bool isInvalidTime = (thoiGianChamSoc + (existing.ThoiGian - currentTime) >= 660 && thoiGianChamSoc <= 780) || thoiGianChamSoc > 1020;

                                if (isInvalidTime)
                                {
                                    hoaDon.ThoiGianChamSoc = 0;
                                }
                            }
                        }

                        await _context.SaveChangesAsync(); // Lưu thay đổi cho HoaDonDichVu
                    }
                }
                return changes > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<DichVu>> List()
        {
            var list = await _context.DichVu.Include(x => x.AnhDichVu)
                                            .Include(x => x.BangGiaDV)
                                                .ThenInclude(x => x.ChiTietBangGiaDV)
                                            .Where(x => x.TrangThai == 0)
                                            .ToListAsync();
            return list;
        }
    }
}
