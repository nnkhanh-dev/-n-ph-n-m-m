using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemHoaDonSPKHService : IHoaDonSPKHService
    {
        private readonly AppDbContext _context;

        public ItemHoaDonSPKHService(AppDbContext context)
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
            {
                return false;
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if(model.PhuongThucThanhToan == 1)
                    {
                        model.TrangThai = -100;
                    }
                    else
                    {
                        model.TrangThai = 0;
                    }

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
                    if(model.PhuongThucThanhToan == 0)
                    {
                        foreach (var item in chiTiet)
                        {
                            var sanpham = await _context.SanPham.FindAsync(item.IdSanPham);
                            if (sanpham == null)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                            if (sanpham.SoLuong < item.SoLuong)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                            sanpham.SoLuong = sanpham.SoLuong - item.SoLuong;
                            _context.SanPham.Update(sanpham);
                            int sanPhamResult = await _context.SaveChangesAsync();
                            if (sanPhamResult == 0)
                            {
                                await transaction.RollbackAsync();
                                return false;
                            }
                        }
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

        public async Task<IEnumerable<HoaDonSanPham>> ListByCustomer(string id)
        {
            var list = await _context.HoaDonSanPham.Include(x => x.ChiTietHoaDonSanPham)
                                                 .Include(x => x.DanhGia)
                                                 .Where(x => x.IdKhachHang == id && x.TrangThai != -100)
                                                 .ToListAsync();
            return list;
        }
    }
}
