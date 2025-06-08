using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Areas.Customer.Services;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public class ItemHoaDonSanPhamAdminService : IHoaDonSanPhamAdminService
    {
        private readonly AppDbContext _context;

        public ItemHoaDonSanPhamAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Cancel(int id)
        {
            if (id == null)
            {
                return false;
            }
            try
            {
                var hoaDon = await _context.HoaDonSanPham.FindAsync(id);
                if (hoaDon == null)
                {
                    return false;
                }
                hoaDon.TrangThai = -1;
                _context.HoaDonSanPham.Update(hoaDon);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Create(HoaDonSanPham model)
        {
            if (model == null)
                return false;

            // Lấy strategy để bao toàn bộ giao dịch
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        model.TrangThai = (model.PhuongThucThanhToan == 1) ? -100 : 0;

                        var chiTiet = model.ChiTietHoaDonSanPham;
                        model.ChiTietHoaDonSanPham = new List<ChiTietHoaDonSanPham>();

                        _context.HoaDonSanPham.Add(model);
                        int hoaDonResult = await _context.SaveChangesAsync();

                        if (hoaDonResult == 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }

                        foreach (var item in chiTiet)
                        {
                            item.IdHoaDon = model.Id;
                            _context.ChiTietHoaDonSanPham.Add(item);
                        }

                        int chiTietResult = await _context.SaveChangesAsync();
                        if (chiTietResult == 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }

                        if (model.PhuongThucThanhToan == 0)
                        {

                            model.PhuongThucThanhToan = 1;
                        }

                        await transaction.CommitAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine(ex);
                        return false;
                    }
                }
            });
        }


        public async Task<HoaDonSanPham> Details(int id)
        {
            var hoaDon = await _context.HoaDonSanPham.Include(x => x.ChiTietHoaDonSanPham)
                                                        .ThenInclude(x => x.SanPham)
                                                            .ThenInclude(x => x.AnhSanPham)
                                                    .Include(x => x.KhachHang)
                                                    .Include(x => x.DanhGia)
                                                        .ThenInclude(x => x.TepDinhKem)
                                                    .FirstOrDefaultAsync(x => x.Id == id);
            return hoaDon;
        }

        public async Task<IEnumerable<HoaDonSanPhamDTO>> List()
        {
            var list = await _context.HoaDonSanPham
                .Include(x => x.ChiTietHoaDonSanPham)
                .Include(x => x.DanhGia)
                .Include(x => x.KhachHang)
                .OrderByDescending(x => x.NgayTao) 
                .Select(x => new HoaDonSanPhamDTO
                {
                    Id = x.Id,
                    HoTenKhach = x.KhachHang.HoTen,
                    TrangThai = x.TrangThai,
                    ChiTietHoaDonSanPham = x.ChiTietHoaDonSanPham,
                    NgayTao = x.NgayTao,
                    IdKhachHang = x.IdKhachHang,
                })
                .ToListAsync();

            return list;
        }


        public async Task<IEnumerable<HoaDonSanPham>> ListByCustomer(string id)
        {
            var list = await _context.HoaDonSanPham.Include(x => x.ChiTietHoaDonSanPham)
                                                    .Include(x => x.DanhGia)
                                                    .Where(x => x.IdKhachHang == id && x.TrangThai != -100)
                                                    .ToListAsync();
            return list;
        }

        public async Task<bool> UpdateByStatus(string id, string status)
        {

            try
            {
                var hoadon = await _context.HoaDonSanPham
                    .FirstOrDefaultAsync(x => x.Id == Int32.Parse(id));

                if (hoadon == null)
                    return false;


                hoadon.TrangThai = Int32.Parse(status);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStatus(string id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var hoadon = await _context.HoaDonSanPham
                        .Include(x => x.ChiTietHoaDonSanPham)
                        .FirstOrDefaultAsync(x => x.MaThanhToan == (id));

                    if (hoadon == null)
                        return false;

                    var sp = await _context.ChiTietHoaDonSanPham
                        .Include(x => x.SanPham)
                        .Where(x => x.IdHoaDon == hoadon.Id)
                        .ToListAsync();

                    foreach (var item in sp)
                    {
                        item.SanPham.SoLuong -= item.SoLuong;
                    }

                    hoadon.TrangThai = 3;
                    hoadon.PhuongThucThanhToan = 0;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }


        public async Task<bool> UpdateSuccessStatus(string id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var hoadon = await _context.HoaDonSanPham
                        .Include(x => x.ChiTietHoaDonSanPham)
                        .FirstOrDefaultAsync(x => x.Id == Int32.Parse(id));

                    if (hoadon == null)
                        return false;

                    var sp = await _context.ChiTietHoaDonSanPham
                        .Include(x => x.SanPham)
                        .Where(x => x.IdHoaDon == hoadon.Id)
                        .ToListAsync();

                    foreach (var item in sp)
                    {
                        item.SanPham.SoLuong -= item.SoLuong;
                    }

                    hoadon.TrangThai = 3;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw; // Rethrow để EF có thể retry nếu cần
                }
            });
        }

    }
}
