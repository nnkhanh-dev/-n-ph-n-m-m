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

 
    }
}
